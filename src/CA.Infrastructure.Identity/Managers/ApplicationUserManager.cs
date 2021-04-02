using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace CA.Infrastructure.Identity.Managers
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
    }
}
