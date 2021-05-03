using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Application.Contracts.DataTransferObjects;

namespace Core.Application.Contracts.Permissions
{
    public static class PermissionHelper
    {
        public static List<ManageClaimDto> GetAllPermissions()
        {
            var allPermissions = new List<ManageClaimDto>();
            var permissionClass = typeof(Permissions);
            var allModulesPermissions = permissionClass.GetNestedTypes().Where(x => x.IsClass).ToList();
            foreach (var modulePermissions in allModulesPermissions)
            {
                var permissions = modulePermissions.GetFields(BindingFlags.Static | BindingFlags.Public);
                allPermissions.AddRange(permissions.Select(permission => new ManageClaimDto
                {
                    Type = CustomClaimTypes.Permission,
                    Value = permission.GetValue(null).ToString(),
                    Checked = false
                }));
            }

            return allPermissions;
        }
     }
}
