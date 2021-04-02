using System.Threading.Tasks;
using CA.Infrastructure.Identity.Seeds;
using CA.Infrastructure.Persistence.Seeds;
using Microsoft.Extensions.Hosting;

namespace CA.Web.Framework.Extensions
{
    public static class GlobalMigrationManager
    {
        public static async Task<IHost> MigrateAndSeedAsync(this IHost host)
        {
            await PersistenceMigrationManager.MigrateDatabaseAsync(host);
            await IdentityMigrationManager.MigrateDatabaseAsync(host);
            await IdentityMigrationManager.SeedDatabaseAsync(host);
            return host;
        }
    }
}
