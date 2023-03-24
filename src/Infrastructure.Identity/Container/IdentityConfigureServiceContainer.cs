using Core.Domain.Identity.Entities;
using Core.Domain.Identity.Interfaces;
using Infrastructure.Identity.Context;
using Infrastructure.Identity.Identity;
using Infrastructure.Identity.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Identity.Container
{
    public static class IdentityConfigureServiceContainer
    {
        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    Environment.GetEnvironmentVariable("PersistenceConnection") ?? 
                    configuration.GetConnectionString("IdentityConnection"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            

        }

        public static void AddManagers(IServiceCollection services)
        {
            #region IdentityManagers
            services.AddTransient<IApplicationUserManager, ApplicationUserManager>();
            services.AddTransient<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddTransient<IApplicationSignInManager, ApplicationSignInManager>();
            #endregion
        }
    }
}
