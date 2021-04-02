using CA.Core.Domain.Identity.Entities;
using CA.Infrastructure.Identity.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

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
                await roleManager.CreateAsync(new ApplicationRole("SuperAdmin"));
                await roleManager.CreateAsync(new ApplicationRole("Admin"));
                await roleManager.CreateAsync(new ApplicationRole("Moderator"));
                await roleManager.CreateAsync(new ApplicationRole("Basic"));
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
            var user = await userManager.FindByNameAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "masum");
                await userManager.AddToRoleAsync(defaultUser, "SuperAdmin");
                await userManager.AddToRoleAsync(defaultUser, "Admin");
                await userManager.AddToRoleAsync(defaultUser, "Moderator");
                await userManager.AddToRoleAsync(defaultUser, "Basic");
            }
        }
    }
}
