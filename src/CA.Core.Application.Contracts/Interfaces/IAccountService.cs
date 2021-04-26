using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CA.Core.Application.Contracts.DataTransferObjects;
using CA.Core.Application.Contracts.Response;

namespace CA.Core.Application.Contracts.Interfaces
{
    public interface IAccountService
    {
        Task<Response<UserIdentityDto>> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<Response<UserIdentityDto>> CookieSignInAsync(LoginUserDto loginUserDto);

        Task<Response<IList<Claim>>> GetAllClaims(ClaimsPrincipal claimsPrincipal);
    }
}
