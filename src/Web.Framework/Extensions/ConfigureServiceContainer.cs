using AutoMapper;
using Core.Application.Container;
using Core.Application.Contracts.Interfaces;
using Core.Domain.Identity.Permissions;
using Infrastructure.Identity.Container;
using Infrastructure.Persistence.Container;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Framework.Authorization;
using Web.Framework.Permissions;
using Web.Framework.Services;

namespace Web.Framework.Extensions
{
    public static class ConfigureServiceContainer
    {
        public static void AddAutoMapper(this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(cfg =>
                    cfg.AddMaps(new[] {
                        "Web.Framework",
                        "Core.Application",
                        "Core.Application.Contracts",
                    })
                );
            IMapper mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);
        }

        public static void AddFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            PersistenceConfigureServiceContainer.AddDbContext(services, configuration);
            PersistenceConfigureServiceContainer.AddRepositories(services);

            IdentityConfigureServiceContainer.AddDbContext(services, configuration);
            IdentityConfigureServiceContainer.AddManagers(services);

            ApplicationConfigureServiceContainer.AddServices(services);

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddSingleton<IPermissionHelper, PermissionHelper>();
        }

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            #region API Versioning
            // Add API Versioning to the Project
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
            #endregion
        }
    }
}
