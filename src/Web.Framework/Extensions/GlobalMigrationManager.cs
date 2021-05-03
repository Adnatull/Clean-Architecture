using Infrastructure.Identity.Seeds;
using Infrastructure.Persistence.Seeds;
using Microsoft.Extensions.Hosting;

namespace Web.Framework.Extensions
{
    public static class GlobalMigrationManager
    {
        public static  IHost MigrateAndSeed(this IHost host)
        {
            PersistenceMigrationManager.MigrateDatabaseAsync(host).GetAwaiter().GetResult();
            IdentityMigrationManager.MigrateDatabaseAsync(host).GetAwaiter().GetResult();
            IdentityMigrationManager.SeedDatabaseAsync(host).GetAwaiter().GetResult();
            return host;
        }
    }
}
