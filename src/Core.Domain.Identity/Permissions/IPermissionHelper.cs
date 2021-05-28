using System.Collections.Generic;
using System.Security.Claims;

namespace Core.Domain.Identity.Permissions
{
    public interface IPermissionHelper
    {
        List<Claim> GetAllPermissions();
    }
}
