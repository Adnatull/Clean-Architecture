using Core.Application.Contracts.Interfaces;
using Core.Domain.Identity.Constants;
using Infrastructure.Persistence.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var dateTime = new Mock<IDateTimeService>(MockBehavior.Strict);
            dateTime.Setup(r => r.NowUtc).Returns(DateTime.UtcNow);

            var authUser = new Mock<ICurrentUser>(MockBehavior.Strict);
            authUser.Setup(r => r.UserId).Returns(DefaultApplicationUsers.GetSuperUser().Id);
            authUser.Setup(r => r.UserName).Returns(DefaultApplicationUsers.GetSuperUser().UserName);
            authUser.Setup(r => r.Roles).Returns(DefaultApplicationRoles.GetDefaultRoles().Select(x => x.Name).ToList());

            var dbContext = new AppDbContext(options, dateTime.Object, authUser.Object);
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            return dbContext;
        }
    }
}
