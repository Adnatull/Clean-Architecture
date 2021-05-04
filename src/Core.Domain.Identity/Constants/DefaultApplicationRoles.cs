using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Core.Domain.Identity.Entities;

namespace Core.Domain.Identity.Constants
{
    public static class DefaultApplicationRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        public const string Basic = "Basic";

        public static List<ApplicationRole> GetDefaultRoles()
        {
            var roles = new List<ApplicationRole>
            {
                new(SuperAdmin),
                new(Admin),
                new(Moderator),
                new(Basic)
            };
            return roles;
        }

        public static List<Claim> GetDefaultRoleClaims()
        {
            var roles = GetDefaultRoles();
            var claims = roles.Select(role => new Claim(ClaimTypes.Role, role.Name)).ToList();
            return claims;
        }
    }
}
