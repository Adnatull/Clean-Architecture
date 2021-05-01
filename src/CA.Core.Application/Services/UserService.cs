using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Application.Contracts.Response;
using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CA.Core.Application.Services
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

        public async Task<Response<ManageUserRolesDto>> ManageRolesAsync(string userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if(user == null) return Response<ManageUserRolesDto>.Fail("No User Exists");
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
            if(user == null)
                return Response<UserIdentityDto>.Fail("No user exists by this id");
            var existingRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, existingRoles.ToList());
            if(!removeResult.Succeeded)
                return Response<UserIdentityDto>.Fail("Failed to remove existing roles");
            var rs = await _userManager.AddToRolesAsync(user,
                manageUserRolesDto.ManageRolesDto.Where(x => x.Checked).Select(x => x.Name).ToList());
            return rs.Succeeded
                ? Response<UserIdentityDto>.Success(new UserIdentityDto {Id = manageUserRolesDto.UserId}, rs.Message)
                : Response<UserIdentityDto>.Fail(rs.Message);
        }
    }
}
