using System;

namespace Core.Application.Contracts.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
