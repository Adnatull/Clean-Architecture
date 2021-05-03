using Core.Application.Contracts.Permissions;
using Core.Domain.Identity.Constants;
using Core.Domain.Identity.Entities;
using Infrastructure.Identity.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
    public static class IdentityMigrationManager
    {
        public static async Task MigrateDatabaseAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            await using var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            try
            {
               await identityContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ". " + ex.Source);
                throw;
            }
        }

        public static async Task SeedDatabaseAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            try
            {

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                await SeedDefaultUserRolesAsync(userManager, roleManager);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ". " + ex.Source);
                throw;
            }
        }

        private static async Task SeedDefaultUserRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.SuperAdmin));
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.Admin));
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.Moderator));
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.Basic));
            }

            if (!await roleManager.RoleExistsAsync(DefaultApplicationRoles.SuperAdmin))
            {
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.SuperAdmin));
            }

            var defaultUser = DefaultApplicationUsers.GetSuperUser();
            var userByName = await userManager.FindByNameAsync(defaultUser.UserName);
            var userByEmail = await userManager.FindByEmailAsync(defaultUser.Email);
            if (userByName == null && userByEmail == null)
            {
                await userManager.CreateAsync(defaultUser, "SuperAdmin");
                await userManager.AddToRoleAsync(defaultUser, DefaultApplicationRoles.SuperAdmin);
                await userManager.AddToRoleAsync(defaultUser, DefaultApplicationRoles.Admin);
                await userManager.AddToRoleAsync(defaultUser, DefaultApplicationRoles.Moderator);
                await userManager.AddToRoleAsync(defaultUser, DefaultApplicationRoles.Basic);
            }

            var role = await roleManager.FindByNameAsync(DefaultApplicationRoles.SuperAdmin);
            var rolePermissions = await roleManager.GetClaimsAsync(role);
            var allPermissions = PermissionHelper.GetAllPermissions();
            foreach (var permission in allPermissions)
            {
                if (rolePermissions.Any(x => x.Value == permission.Value && x.Type == CustomClaimTypes.Permission) == false)
                {
                    await roleManager.AddClaimAsync(role,
                        new Claim(CustomClaimTypes.Permission, permission.Value));
                }
            }
        }
    }
}
