using System.Collections.Generic;
using System.Security.Claims;
using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using CA.Core.Domain.Identity.Response;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CA.Infrastructure.Identity.Extensions;

namespace CA.Infrastructure.Identity.Managers
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResponse> RegisterUserAsync(ApplicationUser user)
        {
            if (user.Email != null && await _userManager.FindByEmailAsync(user.Email) != null)
            {
                return IdentityResponse.Fail("Email already exists! Please try a different one!");
            }
            user.UserName ??= user.Email;
            if (user.UserName != null && await _userManager.FindByNameAsync(user.UserName) != null)
            {
                return IdentityResponse.Fail("User Name already exists! Please try a different one");
            }
            var rs = await _userManager.CreateAsync(user);
            return rs.ToIdentityResponse();
        }

        public async Task<ApplicationUser> GetUserByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await _userManager.GetUserAsync(claimsPrincipal);
        }
    }
}
