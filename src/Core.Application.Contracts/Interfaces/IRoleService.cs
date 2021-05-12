using System.Threading.Tasks;
using Core.Application.Contracts.DataTransferObjects;
using Core.Application.Contracts.Response;

namespace Core.Application.Contracts.Interfaces
{
    public interface IRoleService
    {
        Task<Response<PaginatedList<RoleDto>>> GetPaginatedRolesAsync(int? pageNumber, int? pageSize);
        Task<Response<string>> AddRoleAsync(AddRoleDto addRoleDto);
        Task<Response<ManageRolePermissionsDto>> ManagePermissionsAsync(string roleId, string permissionValue, int? pageNumber, int? pageSize);
        Task<Response<RoleIdentityDto>> ManageRoleClaimAsync(ManageRoleClaimDto manageRoleClaimDto);
    }
}
