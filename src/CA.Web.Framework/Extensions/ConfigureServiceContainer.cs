using AutoMapper;
using CA.Core.Application.Container;
using CA.Core.Application.Contracts.Interfaces;
using CA.Infrastructure.Identity.Container;
using CA.Infrastructure.Persistence.Container;
using CA.Web.Framework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Web.Framework.Extensions
{
    public static class ConfigureServiceContainer
    {
        public static void AddAutoMapper(this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(cfg =>
                    cfg.AddMaps(new[] {
                        "CA.Web.Framework",
                        "CA.Core.Application",
                        "CA.Core.Application.Contracts",
                    })
                );
            IMapper mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);
        }

        public static void AddFramework(this IServiceCollection services, IConfiguration configuration)
        {
            PersistenceConfigureServiceContainer.AddDbContext(services, configuration);
            PersistenceConfigureServiceContainer.AddRepositories(services);

            IdentityConfigureServiceContainer.AddDbContext(services, configuration);
            IdentityConfigureServiceContainer.AddManagers(services);

            ApplicationConfigureServiceContainer.AddServices(services);

            services.AddHttpContextAccessor();
            services.AddTransient<IAuthenticatedUser, AuthenticatedUser>();
            services.AddTransient<IDateTimeService, DateTimeService>();
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
