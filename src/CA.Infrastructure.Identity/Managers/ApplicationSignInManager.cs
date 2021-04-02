using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
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
    }
}
