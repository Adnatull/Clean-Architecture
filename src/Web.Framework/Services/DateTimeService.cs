using System;
using Core.Application.Contracts.Interfaces;

namespace Web.Framework.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
