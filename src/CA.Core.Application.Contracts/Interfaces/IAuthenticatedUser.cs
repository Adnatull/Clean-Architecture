using System.Collections.Generic;

namespace CA.Core.Application.Contracts.Interfaces
{
    public interface IAuthenticatedUser
    {
        string UserId { get; }
        string UserName { get; }
        List<string> Roles { get; }
    }
}
