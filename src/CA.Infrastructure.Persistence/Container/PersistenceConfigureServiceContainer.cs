using CA.Core.Domain.Persistence.Contracts;
using CA.Infrastructure.Persistence.Context;
using CA.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Infrastructure.Persistence.Container
{
    public static class PersistenceConfigureServiceContainer
    {
        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(
                    configuration.GetConnectionString("PersistenceConnectionForMysqlDb"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        }
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IPersistenceUnitOfWork, PersistenceUnitOfWork>();
            services.AddTransient<IPostRepositoryAsync, PostRepositoryAsync>();
            services.AddTransient<ICategoryRepositoryAsync, CategoryRepositoryAsync>();
            services.AddTransient<ITagRepositoryAsync, TagRepositoryAsync>();
        }
    }
}
