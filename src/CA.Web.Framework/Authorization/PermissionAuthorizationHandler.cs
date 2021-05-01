using System.Linq;
using System.Threading.Tasks;
using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Application.Contracts.Permissions;
using CA.Core.Domain.Identity.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CA.Web.Framework.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {

        private readonly ICurrentUser _currentUser;

        public PermissionAuthorizationHandler(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            //var roles = _currentUser.Roles;
            //if (roles.Count > 0 && roles.Contains(DefaultApplicationRoles.SuperAdmin.ToString()))
            //{
            //    context.Succeed(requirement);
            //    return;
            //}
            var permissions = await _currentUser.Permissions();
            if (permissions == null || permissions.Count == 0)
            {
                context.Fail();
                return;
            }

            if (permissions.Any(x => x.Type == CustomClaimTypes.Permission 
                                     && x.Value == requirement.Permission 
                                     && x.Issuer == "LOCAL AUTHORITY"))
            {
                context.Succeed(requirement);
                return;
            }
            context.Fail();
        }
    }
}
