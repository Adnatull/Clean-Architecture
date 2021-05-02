using CA.Core.Domain.Identity.Response;
using System.Threading.Tasks;
using CA.Core.Domain.Identity.Entities;

namespace CA.Core.Domain.Identity.Contracts
{
    public interface IApplicationSignInManager
    {
        Task<IdentityResponse> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent,
            bool lockoutOnFailure);

        Task SignOutAsync();
    }
}
