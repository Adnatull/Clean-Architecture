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
    public class RoleService : IRoleService
    {
        private readonly IApplicationRoleManager _roleManager;
        private readonly IMapper _mapper;
        private readonly IPermissionHelper _permissionHelper;

        public RoleService(IApplicationRoleManager roleManager, IMapper mapper, IPermissionHelper permissionHelper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _permissionHelper = permissionHelper;
        }

        public async Task<Response<PaginatedList<RoleDto>>> GetPaginatedRolesAsync(int? pageNumber, int? pageSize)
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<ApplicationRole, RoleDto>());
            var roles = _roleManager.Roles().ProjectTo<RoleDto>(configuration);
            var rs = await PaginatedList<RoleDto>.CreateFromEfQueryableAsync(roles.AsNoTracking(),
                pageNumber ?? 1, pageSize ?? 12);
            return Response<PaginatedList<RoleDto>>.Success(rs, "Succeeded");
        }

        public async Task<Response<string>> AddRoleAsync(AddRoleDto addRoleDto)
        {
            if (await _roleManager.GetRoleAsync(addRoleDto.Name) != null)
                return Response<string>.Fail("The role already exists. Please try a different one!");
            var appRole = _mapper.Map<ApplicationRole>(addRoleDto);
            var rs = await _roleManager.AddRoleAsync(appRole);
            return rs.Succeeded
                ? Response<string>.Success(appRole.Id, "New role has been created")
                : Response<string>.Fail("Failed to create new role");
        }

        public async Task<Response<ManageRolePermissionsDto>> ManagePermissionsAsync(string roleId, string permissionValue, int? pageNumber, int? pageSize)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return Response<ManageRolePermissionsDto>.Fail("No Role Exists");
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var allPermissions = _permissionHelper.GetAllPermissions();
            
            if (!string.IsNullOrWhiteSpace(permissionValue))
            {
                allPermissions = allPermissions.Where(x => x.Value.ToLower().Contains(permissionValue.Trim().ToLower())).ToList();
            }
            var managePermissionsClaim = new List<ManageClaimDto>();
            foreach (var permission in allPermissions)
            {
                var managePermissionClaim = new ManageClaimDto {Type = permission.Type, Value = permission.Value};
                if (roleClaims.Any(x => x.Value == permission.Value))
                {
                    managePermissionClaim.Checked = true;
                }
                managePermissionsClaim.Add(managePermissionClaim);
            }
            var paginatedList = PaginatedList<ManageClaimDto>.CreateFromLinqQueryable(managePermissionsClaim.AsQueryable(),
                pageNumber ?? 1, pageSize ?? 12);
            var manageRolePermissionsDto = new ManageRolePermissionsDto
            {
                RoleId = roleId,
                RoleName = role.Name,
                PermissionValue = permissionValue,
                ManagePermissionsDto = paginatedList
            };
            return allPermissions.Count > 0
                ? Response<ManageRolePermissionsDto>.Success(manageRolePermissionsDto, "Successfully retrieved")
                : Response<ManageRolePermissionsDto>.Fail(
                    $"No Permissions exists! Something is Wrong with Permission source file");
        }
        public async Task<Response<RoleIdentityDto>> ManageRoleClaimAsync(ManageRoleClaimDto manageRoleClaimDto)
        {
            var roleById = await _roleManager.FindByIdAsync(manageRoleClaimDto.RoleId);
            var roleByName = await _roleManager.GetRoleAsync(manageRoleClaimDto.RoleName);
            if(roleById != roleByName)
                return Response<RoleIdentityDto>.Fail("Forbidden");
            var allClaims = await _roleManager.GetClaimsAsync(roleById);
            var claimExists =
                allClaims.Where(x => x.Type == manageRoleClaimDto.Type && x.Value == manageRoleClaimDto.Value).ToList();
            switch (manageRoleClaimDto.Checked)
            {
                case true when claimExists.Count == 0:
                    await _roleManager.AddClaimAsync(roleById, new Claim(manageRoleClaimDto.Type, manageRoleClaimDto.Value));
                    break;
                case false when claimExists.Count > 0:
                {
                    foreach (var claim in claimExists)
                    {
                        await _roleManager.RemoveClaimAsync(roleById, claim);
                    }
                    break;
                }
            }
            return Response<RoleIdentityDto>.Success(new RoleIdentityDto { RoleId = manageRoleClaimDto.RoleId },
                "Succeeded"); 

        }
    }
}
