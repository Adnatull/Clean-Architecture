using System.Threading.Tasks;
using Core.Application.Contracts.DataTransferObjects;
using Core.Application.Contracts.Response;

namespace Core.Application.Contracts.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedList<UserDto>> GetPaginatedUsersAsync(int? pageNumber, int? pageSize);
        Task<Response<UserDto>> GetUserByIdAsync(string userId);
        Task<Response<ManageUserRolesDto>> ManageRolesAsync(string userId);
        Task<Response<UserIdentityDto>> ManageRolesAsync(ManageUserRolesDto manageUserRolesDto);
        Task<Response<ManageUserPermissionsDto>> ManagePermissionsAsync(string userId);
        Task<Response<UserIdentityDto>> ManagePermissionsAsync(ManageUserPermissionsDto manageUserPermissionsDto);
    }
}
