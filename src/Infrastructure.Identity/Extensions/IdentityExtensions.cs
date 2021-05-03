using Core.Domain.Identity.Response;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Extensions
{
    public static class IdentityExtensions
    {
        public static IdentityResponse ToIdentityResponse(this IdentityResult identityResult)
        {
            return identityResult.Succeeded
                ? IdentityResponse.Success(identityResult.ToString())
                : IdentityResponse.Fail(identityResult.ToString());
        }
    }
}
