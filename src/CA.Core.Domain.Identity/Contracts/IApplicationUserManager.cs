using System.Threading.Tasks;
using CA.Core.Domain.Identity.Entities;
using CA.Core.Domain.Identity.Response;

namespace CA.Core.Domain.Identity.Contracts
{
    public interface IApplicationUserManager
    {
        Task<IdentityResponse> RegisterUser(ApplicationUser user);
    }
}
