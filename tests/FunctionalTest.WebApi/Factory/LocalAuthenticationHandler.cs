using Core.Domain.Identity.Constants;
using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Permissions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FunctionalTest.WebApi.Factory
{
    public class LocalAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly ApplicationUser _appUser = DefaultApplicationUsers.GetSuperUser();

        public LocalAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        private List<Claim> UserClaims()
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, _appUser.Id),
                new(ClaimTypes.Name, _appUser.UserName)
            };
            return claims;
        }
        private List<Claim> AllRolesClaims()
        {
            return DefaultApplicationRoles.GetDefaultRoleClaims();
        }

        private List<Claim> AllPermissionsClaims()
        {
            IPermissionHelper permissions = new Web.Framework.Permissions.PermissionHelper();
            return permissions.GetAllPermissions();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            var allClaims = new List<Claim>();
            allClaims.AddRange(UserClaims());
            allClaims.AddRange(AllRolesClaims());
            allClaims.AddRange(AllPermissionsClaims());
            var authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(new ClaimsIdentity(allClaims, IdentityConstants.ApplicationScheme)),
                new AuthenticationProperties(),
                IdentityConstants.ApplicationScheme);
            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }
}
