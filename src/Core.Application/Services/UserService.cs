using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Application.Contracts.DataTransferObjects;
using Core.Application.Contracts.Interfaces;
using Core.Application.Contracts.Permissions;
using Core.Application.Contracts.Response;
using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IMapper _mapper;

        public UserService(IApplicationUserManager userManager, IApplicationRoleManager roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<PaginatedList<UserDto>> GetPaginatedUsersAsync(int? pageNumber, int? pageSize)
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ApplicationUser, UserDto>());
            var users = _userManager.Users().ProjectTo<UserDto>(configuration);
            return await PaginatedList<UserDto>.CreateAsync(users.AsNoTracking(),
                pageNumber ?? 1, pageSize ?? 12);
        }

        public async Task<Response<UserDto>> GetUserByIdAsync(string userId)
        {
            var appUser = await _userManager.GetUserByIdAsync(userId);
            if (appUser == null) return Response<UserDto>.Fail("User does not exists");
            var userDto = _mapper.Map<UserDto>(appUser);
            return Response<UserDto>.Success(userDto, "Retrieved successfully");
        }

        public async Task<Response<IList<Claim>>> GetAllClaims(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = await _roleManager.GetClaimsAsync(roles);

            var claims = userClaims.Union(roleClaims).ToList();
            return claims.Count > 0
                ? Response<IList<Claim>>.Success(claims, "Successfully retrieved")
                : Response<IList<Claim>>.Fail("No Claims found");
        }

        public async Task<Response<IList<string>>> GetRolesAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Count > 0
                ? Response<IList<string>>.Success(roles, "Successfully retrieved")
                : Response<IList<string>>.Fail("No Roles found");
        }

        public async Task<Response<ManageUserRolesDto>> ManageRolesAsync(string userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null) return Response<ManageUserRolesDto>.Fail("No User Exists");
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles().ToListAsync();
            var allRolesDto = _mapper.Map<List<ManageRoleDto>>(allRoles);
            foreach (var roleDto in allRolesDto.Where(roleDto => userRoles.Contains(roleDto.Name)))
            {
                roleDto.Checked = true;
            }

            var manageUserRolesDto = new ManageUserRolesDto
            {
                UserId = userId,
                UserName = user.UserName,
                ManageRolesDto = allRolesDto
            };
            return allRolesDto.Count > 0
                ? Response<ManageUserRolesDto>.Success(manageUserRolesDto, "Success")
                : Response<ManageUserRolesDto>.Fail("No roles found");
        }

        public async Task<Response<UserIdentityDto>> ManageRolesAsync(ManageUserRolesDto manageUserRolesDto)
        {
            var user = await _userManager.GetUserByIdAsync(manageUserRolesDto.UserId);
            if (user == null)
                return Response<UserIdentityDto>.Fail("No user exists by this id");
            var existingRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, existingRoles.ToList());
            if (!removeResult.Succeeded)
                return Response<UserIdentityDto>.Fail("Failed to remove existing roles");
            var rs = await _userManager.AddToRolesAsync(user,
                manageUserRolesDto.ManageRolesDto.Where(x => x.Checked).Select(x => x.Name).ToList());
            return rs.Succeeded
                ? Response<UserIdentityDto>.Success(new UserIdentityDto { Id = manageUserRolesDto.UserId }, rs.Message)
                : Response<UserIdentityDto>.Fail(rs.Message);
        }

        public async Task<Response<ManageUserPermissionsDto>> ManagePermissionsAsync(string userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null) return Response<ManageUserPermissionsDto>.Fail("No User Exists");
            var userPermissions = await _userManager.GetClaimsAsync(user);
            var allPermissions = PermissionHelper.GetAllPermissions();
            foreach (var permission in allPermissions)
            {
                if (userPermissions.Any(x => x.Value == permission.Value))
                {
                    permission.Checked = true;
                }
            }
            var manageUserPermissionsDto = new ManageUserPermissionsDto
            {
                UserId = userId,
                UserName = user.UserName,
                ManagePermissionsDto = allPermissions
            };
            return allPermissions.Count > 0
                ? Response<ManageUserPermissionsDto>.Success(manageUserPermissionsDto, "Successfully retrieved")
                : Response<ManageUserPermissionsDto>.Fail(
                    $"No Permissions exists! Something is Wrong with {typeof(Permissions).Namespace} file");
        }

        public async Task<Response<UserIdentityDto>> ManagePermissionsAsync(ManageUserPermissionsDto manageUserPermissionsDto)
        {
            var user = await _userManager.GetUserByIdAsync(manageUserPermissionsDto.UserId);
            if (user == null)
                return Response<UserIdentityDto>.Fail("No user exists by this id");

            var existingClaims = await _userManager.GetClaimsAsync(user);
            var existingPermissions = existingClaims.Where(x => x.Type == CustomClaimTypes.Permission).ToList();
            var removeResult = await _userManager.RemoveClaimAsync(user, existingPermissions);
            if (!removeResult.Succeeded)
                return Response<UserIdentityDto>.Fail("Failed to remove existing Permissions");

            var newClaims = manageUserPermissionsDto.ManagePermissionsDto.Where(x => x.Checked)
                .Select(x => new Claim(CustomClaimTypes.Permission, x.Value)).ToList();
            var rs = await _userManager.AddClaimsAsync(user, newClaims);
           
            return rs.Succeeded
                ? Response<UserIdentityDto>.Success(new UserIdentityDto { Id = manageUserPermissionsDto.UserId }, rs.Message)
                : Response<UserIdentityDto>.Fail(rs.Message);
        }
    }
}
