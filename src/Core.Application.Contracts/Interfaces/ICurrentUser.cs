using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Application.Contracts.Interfaces
{
    public interface ICurrentUser
    {
        string UserId { get; }
        string UserName { get; }
        IReadOnlyList<string> Roles { get; }
        Task<IList<Claim>> Permissions();
    }
}
