using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Response;
using System.Threading.Tasks;

namespace CA.Core.Application.Contracts.Interfaces
{
    public interface IRoleService
    {
        Task<PaginatedList<RoleDto>> GetPaginatedRolesAsync(int? pageNumber, int? pageSize);
        Task<Response<string>> AddRoleAsync(AddRoleDto addRoleDto);
    }
}
