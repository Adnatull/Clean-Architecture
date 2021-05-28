using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Application.Contracts.DataTransferObjects;
using Core.Application.Contracts.Interfaces;
using Core.Application.Contracts.Response;
using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Interfaces;
using Core.Domain.Identity.Permissions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IPermissionHelper _permissionHelper;

        public UserService(IApplicationUserManager userManager,
                            IApplicationRoleManager roleManager,
                            IMapper mapper,
                            ICurrentUser currentUser,
                            IPermissionHelper permissionHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _currentUser = currentUser;
            _permissionHelper = permissionHelper;
        }

        public async Task<Response<PaginatedList<UserDto>>> GetPaginatedUsersAsync(int? pageNumber, int? pageSize)
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ApplicationUser, UserDto>());
            var users = _userManager.Users().ProjectTo<UserDto>(configuration);
            var rs = await PaginatedList<UserDto>.CreateFromEfQueryableAsync(users.AsNoTracking(),
                pageNumber ?? 1, pageSize ?? 12);
            return Response<PaginatedList<UserDto>>.Success(rs, "Succeeded");
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
            foreach (var roleDto in manageUserRolesDto.ManageRolesDto)
            {
                var roleExists = existingRoles.FirstOrDefault(x => x == roleDto.Name);
                switch (roleDto.Checked)
                {
                    case true when roleExists == null:
                        await _userManager.AddToRoleAsync(user, roleDto.Name);
                        break;
                    case false when roleExists != null:
                        await _userManager.RemoveFromRoleAsync(user, roleDto.Name);
                        break;
                }
            }
            return Response<UserIdentityDto>.Success(new UserIdentityDto { Id = manageUserRolesDto.UserId }, "Succeeded");
        }

        public async Task<Response<ManageUserPermissionsDto>> ManagePermissionsAsync(string userId, string permissionValue, int? pageNumber, int? pageSize)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null) return Response<ManageUserPermissionsDto>.Fail("No User Exists");
            var userPermissions = await _userManager.GetClaimsAsync(user);
            var allPermissions = _permissionHelper.GetAllPermissions();
            if (!string.IsNullOrWhiteSpace(permissionValue))
            {
                allPermissions = allPermissions.Where(x => x.Value.ToLower().Contains(permissionValue.Trim().ToLower()))
                    .ToList();
            }
            var managePermissionsClaim = new List<ManageClaimDto>();
            foreach (var permission in allPermissions)
            {
                var managePermissionClaim = new ManageClaimDto { Type = permission.Type, Value = permission.Value };
                if (userPermissions.Any(x => x.Value == permission.Value))
                {
                    managePermissionClaim.Checked = true;
                }
                managePermissionsClaim.Add(managePermissionClaim);
            }

            var paginatedList = PaginatedList<ManageClaimDto>.CreateFromLinqQueryable(managePermissionsClaim.AsQueryable(),
                pageNumber ?? 1, pageSize ?? 12);
            var manageUserPermissionsDto = new ManageUserPermissionsDto
            {
                UserId = userId,
                UserName = user.UserName,
                PermissionValue = permissionValue,
                ManagePermissionsDto = paginatedList
            };
            return allPermissions.Count > 0
                ? Response<ManageUserPermissionsDto>.Success(manageUserPermissionsDto, "Successfully retrieved")
                : Response<ManageUserPermissionsDto>.Fail(
                    $"No Permissions exist! Something is Wrong with Permission source file");
        }

        public async Task<Response<UserIdentityDto>> ManageUserClaimAsync(ManageUserClaimDto manageUserClaimDto)
        {
            var userById = await _userManager.GetUserByIdAsync(manageUserClaimDto.UserId);
            var userByName = await _userManager.GetUserByNameAsync(manageUserClaimDto.UserName);

            if (userById != userByName)
                return Response<UserIdentityDto>.Fail("Forbidden");
            var allClaims = await _userManager.GetClaimsAsync(userById);
            var claimExists =
                allClaims.Where(x => x.Type == manageUserClaimDto.Type && x.Value == manageUserClaimDto.Value).ToList();
            switch (manageUserClaimDto.Checked)
            {
                case true when claimExists.Count == 0:
                    {
                        await _userManager.AddClaimAsync(userById,
                            new Claim(manageUserClaimDto.Type, manageUserClaimDto.Value));
                        break;
                    }
                case false when claimExists.Count > 0:
                    {
                        await _userManager.RemoveClaimsAsync(userById, claimExists);
                        break;
                    }
            }
            return Response<UserIdentityDto>.Success(new UserIdentityDto { Id = manageUserClaimDto.UserId },
                "Succeeded"); ;
        }

        public async Task<Response<UserDetailDto>> GetUserDetailByIdAsync(string userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null)
                return Response<UserDetailDto>.Fail("No user exists by this ID");
            var userDetailDto = _mapper.Map<UserDetailDto>(user);
            return Response<UserDetailDto>.Success(userDetailDto, "Successfully retrieved user detail");
        }

        public async Task<Response<UserIdentityDto>> UpdateUserProfile(UserDetailDto userDetailDto)
        {
            if (_currentUser.UserId != userDetailDto.Id)
                return Response<UserIdentityDto>.Fail("You are not authorized to manipulate this user");

            var user = await _userManager.GetUserByIdAsync(userDetailDto.Id);
            if (user == null) return Response<UserIdentityDto>.Fail("Invalid user");
            var userByEmail = await _userManager.FindByEmailAsync(user.Email);
            if (userByEmail != null && userByEmail.Id != user.Id)
                return Response<UserIdentityDto>.Fail("The email address is not available");
            var userByName = await _userManager.GetUserByNameAsync(userDetailDto.UserName);
            if (userByName != null && userByName.Id != user.Id)
                return Response<UserIdentityDto>.Fail("The username is not available");

            _mapper.Map(userDetailDto, user);
            var rs = await _userManager.UpdateAsync(user);
            return rs.Succeeded
                ? Response<UserIdentityDto>.Success(new UserIdentityDto { Id = user.Id }, rs.Message)
                : Response<UserIdentityDto>.Fail(rs.Message);
        }
    }
}
