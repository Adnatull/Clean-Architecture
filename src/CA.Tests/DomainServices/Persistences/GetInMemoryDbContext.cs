using CA.Core.Application.Contracts.Interfaces;
using CA.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using LinqToDB.EntityFrameworkCore;

namespace CA.Test.Unit.DomainServices.Persistences
{
    public class GetInMemoryDbContext
    {
        public static AppDbContext GetMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var dateTime = new Mock<IDateTimeService>(MockBehavior.Strict);
            dateTime.Setup(r => r.NowUtc).Returns(DateTime.UtcNow);

            var authUser = new Mock<IAuthenticatedUser>(MockBehavior.Strict);
            authUser.Setup(r => r.UserId).Returns("1");
            authUser.Setup(r => r.UserName).Returns("A2Masum");
            authUser.Setup(r => r.Roles).Returns(new List<string> {"SuperAdmin"});
            
            return new AppDbContext(options, dateTime.Object, authUser.Object);
        }
    }
}
