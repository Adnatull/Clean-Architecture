using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Helpers
{
    public interface ICurrentUserInfo
    {
        string UserId { get; }
        string UserName { get; }
        IReadOnlyList<string> Roles { get; }
        
    }
}
