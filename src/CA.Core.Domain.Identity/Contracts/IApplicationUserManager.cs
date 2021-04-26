using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using CA.Core.Domain.Identity.Entities;
using CA.Core.Domain.Identity.Response;

namespace CA.Core.Domain.Identity.Contracts
{
    public interface IApplicationUserManager
    {
        Task<IdentityResponse> RegisterUserAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByNameAsync(string userName);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);
    }
}
