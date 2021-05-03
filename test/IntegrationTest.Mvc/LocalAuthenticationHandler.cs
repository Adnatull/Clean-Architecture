using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Core.Application.Contracts.Permissions;
using Core.Domain.Identity.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntegrationTest.Mvc
{
    public class LocalAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public static readonly Guid UserId = Guid.Parse("687d7c63-9a15-4faf-af5a-140782baa24d");

        public static readonly string UserName = "SuperAdmin";

        public LocalAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        private readonly Claim _defaultUserIdClaim = new Claim(
            ClaimTypes.NameIdentifier, UserId.ToString());

        private readonly Claim _defaultUserNameClaim = new Claim(
            ClaimTypes.Name, UserName);
        private List<Claim> AllRolesClaims()
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, DefaultApplicationRoles.SuperAdmin.ToString()),
                new(ClaimTypes.Role, DefaultApplicationRoles.Admin.ToString()),
                new(ClaimTypes.Role, DefaultApplicationRoles.Moderator.ToString()),
                new(ClaimTypes.Role, DefaultApplicationRoles.Basic.ToString())
            };
            return claims;
        }

        private List<Claim> AllPermissionsClaims()
        {
            var allPermissions = PermissionHelper.GetAllPermissions();
            var newClaims = allPermissions.Select(x => new Claim(CustomClaimTypes.Permission, x.Value)).ToList();
            return newClaims;
        }
 
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            var allClaims = new List<Claim>();
            allClaims.Add(_defaultUserIdClaim);
            allClaims.Add(_defaultUserNameClaim);
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
