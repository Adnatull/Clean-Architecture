using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Application.Contracts.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

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
