using System;
using CA.Core.Application.Contracts.Interfaces;

namespace CA.Web.Framework.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
