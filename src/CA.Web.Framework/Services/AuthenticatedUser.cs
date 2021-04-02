using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CA.Core.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CA.Web.Framework.Services
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
        {
            
            UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UserName = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            Roles = httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(x => x.Value.ToString()).ToList();
        }
        public string UserId { get; }
        public string UserName { get; }
        public List<string> Roles { get; }
    }
}
