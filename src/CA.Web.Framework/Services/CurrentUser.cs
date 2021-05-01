using CA.Core.Application.Contracts.Interfaces;
using CA.Web.Framework.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CA.Core.Application.Contracts.Permissions;

namespace CA.Web.Framework.Services
{
    public class CurrentUser : ICurrentUser
    {
        public CurrentUser(IHttpContextAccessor httpContextAccessor, IAccountService accountService)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UserName = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            Roles = httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(x => x.Value.ToString()).ToList();
            Permissions = httpContextAccessor.HttpContext?.User?.Claims.Where(x =>
                x.Type == CustomClaimTypes.Permission && x.Issuer == "LOCAL AUTHORITY").ToList();
        }
        public string UserId { get; }
        public string UserName { get; }
        public IReadOnlyList<string> Roles { get; }
        public IReadOnlyList<Claim> Permissions { get; }
    }
}
