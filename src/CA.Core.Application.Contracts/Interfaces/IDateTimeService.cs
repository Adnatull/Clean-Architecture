using System;

namespace CA.Core.Application.Contracts.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
