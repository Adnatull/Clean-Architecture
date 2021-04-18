using CA.Core.Application.Contracts.Interfaces;
using CA.Infrastructure.Persistence.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CA.UnitTest.Persistence
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

            var authUser = new Mock<IAuthenticatedUser>(MockBehavior.Strict);
            authUser.Setup(r => r.UserId).Returns("");
            authUser.Setup(r => r.UserName).Returns("");
            authUser.Setup(r => r.Roles).Returns(new List<string> { "SuperAdmin" });
            var dbContext = new AppDbContext(options, dateTime.Object, authUser.Object);
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            return dbContext;
        }
    }
}
