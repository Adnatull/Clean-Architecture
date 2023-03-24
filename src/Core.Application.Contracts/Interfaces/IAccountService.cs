using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Application.Contracts.DataTransferObjects;
using Core.Application.Contracts.Response;

namespace Core.Application.Contracts.Interfaces
{
    public interface IAccountService
    {
        Task<Response<UserIdentityDto>> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<Response<UserIdentityDto>> CookieSignInAsync(LoginUserDto loginUserDto);
        Task<Response<UserIdentityDto>> CheckPasswordAsync(LoginUserDtoForApi loginUserDto);
        Task CookieSignOutAsync();
    }
}
