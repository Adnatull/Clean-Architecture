using System.Threading.Tasks;
using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Response;

namespace Core.Domain.Identity.Interfaces
{
    public interface IApplicationSignInManager
    {
        Task<IdentityResponse> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent,
            bool lockoutOnFailure);

        Task SignOutAsync();
    }
}
