using Infrastructure.Identity.Context;
using Infrastructure.Identity.Seeds;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Api;

namespace FunctionalTest.WebApi.Factory
{
    public class AppFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                SetUpMoqDataBase(services);
                services.AddSingleton<IAuthenticationSchemeProvider, MockSchemeProvider>();
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);
            IdentityMigrationManager.SeedDatabaseAsync(host).GetAwaiter().GetResult();
            return host;
        }

        private static void SetUpMoqDataBase(IServiceCollection services)
        {
            SetUpMoqIdentityDatabase(services);
            SetUpMoqPersistenceDatabase(services);
        }

        private static void SetUpMoqIdentityDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<IdentityContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlite()
                .BuildServiceProvider();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlite(connection);
                options.UseInternalServiceProvider(serviceProvider);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            using var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            try
            {
                identityContext.Database.EnsureDeleted();
                identityContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                //Log errors or do anything you think it's needed
                throw;
            }

        }

        private static void SetUpMoqPersistenceDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlite()
                .BuildServiceProvider();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connection);
                options.UseInternalServiceProvider(serviceProvider);
            });
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                appContext.Database.EnsureDeleted();
                appContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                //Log errors or do anything you think it's needed
                throw;
            }
        }


    }
}
