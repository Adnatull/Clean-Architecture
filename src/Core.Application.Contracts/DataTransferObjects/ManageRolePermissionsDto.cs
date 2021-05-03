using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.Contracts.DataTransferObjects
{
    public class ManageRolePermissionsDto
    {
        [Required]
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<ManageClaimDto> ManagePermissionsDto { get; set; }

    }
}
