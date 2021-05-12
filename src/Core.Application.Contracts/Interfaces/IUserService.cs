using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Application.Contracts.DataTransferObjects;
using Core.Application.Contracts.Response;

namespace Core.Application.Contracts.Interfaces
{
    public interface IUserService
    {
        Task<Response<PaginatedList<UserDto>>> GetPaginatedUsersAsync(int? pageNumber, int? pageSize);
        Task<Response<UserDto>> GetUserByIdAsync(string userId);
        Task<Response<IList<Claim>>> GetAllClaims(ClaimsPrincipal claimsPrincipal);
        Task<Response<IList<string>>> GetRolesAsync(ClaimsPrincipal claimsPrincipal);
        Task<Response<ManageUserRolesDto>> ManageRolesAsync(string userId);
        Task<Response<UserIdentityDto>> ManageRolesAsync(ManageUserRolesDto manageUserRolesDto);
        Task<Response<ManageUserPermissionsDto>> ManagePermissionsAsync(string userId, string permissionValue, int? pageNumber, int? pageSize);
        Task<Response<UserIdentityDto>> ManageUserClaimAsync(ManageUserClaimDto manageUserClaimDto);
        Task<Response<UserDetailDto>> GetUserDetailByIdAsync(string userId);
        Task<Response<UserIdentityDto>> UpdateUserProfile(UserDetailDto userDetailDto);
    }
}
