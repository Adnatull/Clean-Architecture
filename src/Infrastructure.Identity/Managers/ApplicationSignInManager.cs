using System.Threading.Tasks;
using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Interfaces;
using Core.Domain.Identity.Response;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Managers
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

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
