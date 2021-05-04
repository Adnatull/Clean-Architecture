using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Application.Contracts.Interfaces;
using Core.Application.Contracts.Permissions;
using Microsoft.AspNetCore.Http;

namespace IntegrationTest.Mvc
{
    public class CurrentUserTest : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        private List<Claim> _permissions;
        public CurrentUserTest(IHttpContextAccessor httpContextAccessor )
        {
            _httpContextAccessor = httpContextAccessor;
            UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UserName = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            Roles = httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(x => x.Value.ToString()).ToList();
        }
        public string UserId { get; }
        public string UserName { get; }
        public IReadOnlyList<string> Roles { get; }
        public async Task<IList<Claim>> Permissions() => await GetPermissions();


        private async Task<IList<Claim>> GetPermissions()
        {
            var permissions = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                                                           x.Issuer == "LOCAL AUTHORITY").ToList();
            return permissions;
        }
    }
}
