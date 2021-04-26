using System.Linq;
using System.Threading.Tasks;
using CA.Core.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CA.Web.Framework.Authorization
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IAccountService _accountService;
        public PermissionAuthorizationHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }
            
            var claims = await _accountService.GetAllClaims(context.User);
            if (!claims.Succeeded)
            {
                context.Fail();
                return;
            }

            if (claims.Data.Any(x => x.Type == CustomClaimTypes.Permission 
                                     && x.Value == requirement.Permission 
                                     && x.Issuer == "LOCAL AUTHORITY"))
            {
                context.Succeed(requirement);
            }
            context.Fail();
        }
    }
}
