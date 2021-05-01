﻿using CA.Core.Application.Contracts.Interfaces;
using CA.Core.Domain.Identity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CA.Web.Framework.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private List<Claim> _permissions;
        public CurrentUser(IHttpContextAccessor httpContextAccessor,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<ApplicationRole> roleManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
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
            if (_permissions != null) return _permissions;
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (user == null)
                return null;
            var userPermissions = await _userManager.GetClaimsAsync(user);
            _permissions = userPermissions.ToList();

            var roleNames = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                _permissions.AddRange(roleClaims);
            }
            return _permissions;
        }
    }
}