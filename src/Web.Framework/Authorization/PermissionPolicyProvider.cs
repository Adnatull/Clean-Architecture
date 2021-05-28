using Core.Domain.Identity.CustomClaims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Web.Framework.Authorization
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(CustomClaimTypes.Permission, StringComparison.OrdinalIgnoreCase))
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            var policy = new AuthorizationPolicyBuilder();
            policy.RequireAuthenticatedUser();
            policy.AddRequirements(new PermissionRequirement(policyName));
            return Task.FromResult(policy.Build());
        }
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
    }
}
