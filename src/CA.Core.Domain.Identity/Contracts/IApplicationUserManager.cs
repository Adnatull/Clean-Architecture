using CA.Core.Domain.Identity.Entities;
using CA.Core.Domain.Identity.Response;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CA.Core.Domain.Identity.Contracts
{
    public interface IApplicationUserManager
    {
        Task<IdentityResponse> RegisterUserAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByNameAsync(string userName);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<IdentityResponse> AddToRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResponse> AddToRolesAsync(ApplicationUser user, List<string> roleNames);
        Task<IdentityResponse> RemoveFromRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResponse> RemoveFromRolesAsync(ApplicationUser user, List<string> roleNames);
        IQueryable<ApplicationUser> Users();

    }
}
