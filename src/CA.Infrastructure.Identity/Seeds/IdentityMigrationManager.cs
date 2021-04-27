using CA.Core.Domain.Identity.Entities;
using CA.Infrastructure.Identity.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using CA.Core.Domain.Identity.Enums;

namespace CA.Infrastructure.Identity.Seeds
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
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.SuperAdmin.ToString()));
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.Admin.ToString()));
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.Moderator.ToString()));
                await roleManager.CreateAsync(new ApplicationRole(DefaultApplicationRoles.Basic.ToString()));
            }
            var defaultUser = new ApplicationUser
            {
                UserName = "masum",
                Email = "a2masum@yahoo.com",
                FirstName = "Al",
                LastName = "Masum",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var userByName = await userManager.FindByNameAsync(defaultUser.UserName);
            var userByEmail = await userManager.FindByEmailAsync(defaultUser.Email);
            if (userByName == null && userByEmail == null)
            {
                await userManager.CreateAsync(defaultUser, "masum");
                await userManager.AddToRoleAsync(defaultUser, DefaultApplicationRoles.SuperAdmin.ToString());
                await userManager.AddToRoleAsync(defaultUser, DefaultApplicationRoles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, DefaultApplicationRoles.Moderator.ToString());
                await userManager.AddToRoleAsync(defaultUser, DefaultApplicationRoles.Basic.ToString());
            }
        }
    }
}
