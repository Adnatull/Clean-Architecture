using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CA.Core.Application.Contracts.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedList<UserDto>> GetPaginatedUsersAsync(int? pageNumber, int? pageSize);
        Task<Response<UserDto>> GetUserByIdAsync(string userId);
        Task<Response<ManageUserRolesDto>> ManageRolesAsync(string userId);
        Task<Response<UserIdentityDto>> ManageRolesAsync(ManageUserRolesDto manageUserRolesDto);
        Task<Response<ManageUserPermissionsDto>> ManagePermissionsAsync(string userId);
    }
}
