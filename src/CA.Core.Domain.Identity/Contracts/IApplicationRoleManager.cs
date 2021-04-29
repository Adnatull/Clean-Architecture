using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CA.Core.Domain.Identity.Entities;

namespace CA.Core.Domain.Identity.Contracts
{
    public interface IApplicationRoleManager
    {
        Task<IList<Claim>> GetClaims(string roleName);
        Task<IList<Claim>> GetClaims(IList<string> roleNames);
        Task<ApplicationRole> GetRole(string roleName);
        IQueryable<ApplicationRole> Roles();
    }
}
