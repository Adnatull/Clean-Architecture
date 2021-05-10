using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Response;
using Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Domain.Identity.Constants;
using Core.Domain.Identity.Interfaces;

namespace Infrastructure.Identity.Managers
{
    public class ApplicationRoleManager : IApplicationRoleManager
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationRoleManager(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IList<Claim>> GetClaimsAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return await _roleManager.GetClaimsAsync(role);
        }

        public async Task<IList<Claim>> GetClaimsAsync(IList<string> roleNames)
        {
            var roles = _roleManager.Roles.Where(x => roleNames.Contains(x.Name)).ToList();
            var allClaims = new List<Claim>();
            
            foreach (var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                allClaims.AddRange(claims);
            }
            return allClaims;
        }

        public async Task<ApplicationRole> GetRoleAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationRole role)
        {
            return await _roleManager.GetClaimsAsync(role);
        }

        public async Task<IdentityResponse> RemoveClaimAsync(ApplicationRole role, Claim claim)
        {
            var rs = await _roleManager.RemoveClaimAsync(role, claim);
            return rs.ToIdentityResponse();
        }

        public async Task<IdentityResponse> AddClaimAsync(ApplicationRole role, Claim claim)
        {
            var rs = await _roleManager.AddClaimAsync(role, claim);
            return rs.ToIdentityResponse();
        }

        public IQueryable<ApplicationRole> Roles()
        {
            return _roleManager.Roles.Where(x => x.Name != DefaultApplicationRoles.SuperAdmin);
        }

        /// <inheritdoc />
        public async Task<IdentityResponse> AddRoleAsync(ApplicationRole applicationRole)
        {
            var rs =  await _roleManager.CreateAsync(applicationRole);
            return rs.ToIdentityResponse();
        }
    }
}
