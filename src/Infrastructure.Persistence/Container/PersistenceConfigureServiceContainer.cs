using Core.Domain.Persistence.Interfaces;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Helpers;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Persistence.Container
{
    public static class PersistenceConfigureServiceContainer
    {
        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Environment.GetEnvironmentVariable("PersistenceConnection") ??
                    configuration.GetConnectionString("PersistenceConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        }
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IPersistenceUnitOfWork, PersistenceUnitOfWork>();
            services.AddTransient<IPostRepositoryAsync, PostRepositoryAsync>();
            services.AddTransient<ICategoryRepositoryAsync, CategoryRepositoryAsync>();
            services.AddTransient<ITagRepositoryAsync, TagRepositoryAsync>();
            services.AddScoped<ICurrentUserInfo, CurrentUserInfo>();
        }
    }
}
