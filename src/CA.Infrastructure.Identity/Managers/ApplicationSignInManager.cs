using System.Threading.Tasks;
using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using CA.Core.Domain.Identity.Response;
using Microsoft.AspNetCore.Identity;

namespace CA.Infrastructure.Identity.Managers
{
    public class ApplicationSignInManager : IApplicationSignInManager
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationSignInManager(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IdentityResponse> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var rs = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            return rs.Succeeded ? IdentityResponse.Success(rs.ToString()) : IdentityResponse.Fail(rs.ToString());
        }
    }
}
