using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Response;

namespace Core.Domain.Identity.Interfaces
{
    public interface IApplicationUserManager
    {
        Task<IdentityResponse> RegisterUserAsync(ApplicationUser user);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> GetUserByNameAsync(string userName);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<IdentityResponse> AddToRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResponse> AddToRolesAsync(ApplicationUser user, List<string> roleNames);
        Task<IdentityResponse> RemoveFromRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResponse> RemoveFromRolesAsync(ApplicationUser user, List<string> roleNames);
        Task<IdentityResponse> AddClaimsAsync(ApplicationUser user, List<Claim> claims);
        Task<IdentityResponse> AddClaimAsync(ApplicationUser user, Claim claim);
        Task<IdentityResponse> RemoveClaimsAsync(ApplicationUser user, List<Claim> claims);
        Task<IdentityResponse> UpdateAsync(ApplicationUser user);
        Task<IdentityResponse> HasClaimAsync(ApplicationUser user, Claim claim);
        Task<IdentityResponse> CheckPasswordAsync(ApplicationUser user, string password);

        IQueryable<ApplicationUser> Users();

    }
}
