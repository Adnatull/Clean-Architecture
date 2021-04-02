using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace CA.Infrastructure.Identity.Managers
{
    public class ApplicationRoleManager : IApplicationRoleManager
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationRoleManager(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
    }
}
