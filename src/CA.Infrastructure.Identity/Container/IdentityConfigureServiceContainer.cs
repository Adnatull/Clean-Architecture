using CA.Core.Domain.Identity.Contracts;
using CA.Core.Domain.Identity.Entities;
using CA.Infrastructure.Identity.Context;
using CA.Infrastructure.Identity.Managers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Infrastructure.Identity.Container
{
    public static class IdentityConfigureServiceContainer
    {
        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 4;

                    }
                ).AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

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
