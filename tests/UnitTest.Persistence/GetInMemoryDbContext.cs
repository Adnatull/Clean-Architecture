using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Identity.Constants;

namespace UnitTest.Persistence
{
    public class GetInMemoryDbContext
    {
        public static async Task<AppDbContext> GetMemoryContext()
        {
            const string inMemoryConnectionString = "DataSource=:memory:";
            var connection = new SqliteConnection(inMemoryConnectionString);
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;



            var authUser = new Mock<ICurrentUserInfo>(MockBehavior.Strict);
            authUser.Setup(r => r.UserId).Returns(DefaultApplicationUsers.GetSuperUser().Id);
            authUser.Setup(r => r.UserName).Returns(DefaultApplicationUsers.GetSuperUser().UserName);
            authUser.Setup(r => r.Roles).Returns(DefaultApplicationRoles.GetDefaultRoles().Select(x => x.Name).ToList());

            var dbContext = new AppDbContext(options, authUser.Object);
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            return dbContext;
        }
    }
}
