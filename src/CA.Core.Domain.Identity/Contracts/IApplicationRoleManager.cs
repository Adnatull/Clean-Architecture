using CA.Core.Domain.Identity.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CA.Core.Domain.Identity.Response;

namespace CA.Core.Domain.Identity.Contracts
{
    public interface IApplicationRoleManager
    {
        Task<IList<Claim>> GetClaimsAsync(string roleName);
        Task<IList<Claim>> GetClaimsAsync(IList<string> roleNames);
        Task<ApplicationRole> GetRoleAsync(string roleName);
        IQueryable<ApplicationRole> Roles();

        /// <summary>
        /// Add new role to database role table.
        /// </summary>
        /// <param name="applicationRole"></param>
        /// <returns>IdentityResponse object.</returns>
        Task<IdentityResponse> AddRoleAsync(ApplicationRole applicationRole);
    }
}
