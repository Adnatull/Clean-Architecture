using Core.Application.Contracts.Response;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.Contracts.DataTransferObjects
{
    public class ManageRolePermissionsDto
    {
        
        [Required]
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string PermissionValue { get; set; }
        public PaginatedList<ManageClaimDto> ManagePermissionsDto { get; set; }

    }
}
