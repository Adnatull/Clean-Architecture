using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using CA.Core.Domain.Identity.Extensions;
using CA.Core.Domain.Identity.Response;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

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
            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                return IdentityResponse.Fail("Email already exists! Please try a different one!");
            }

            if (await _userManager.FindByNameAsync(user.UserName) != null)
            {
                return IdentityResponse.Fail("User Name already exists! Please try a different one");
            }
            var rs = await _userManager.CreateAsync(user);
            return rs.ToIdentityResponse();
        }
    }
}
