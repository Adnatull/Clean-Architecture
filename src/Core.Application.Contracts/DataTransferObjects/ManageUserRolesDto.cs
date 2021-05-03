using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.Contracts.DataTransferObjects
{
    public class ManageUserRolesDto
    {
        [Required]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<ManageRoleDto> ManageRolesDto { get; set; }
    }
    public class ManageRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Checked { get; set; }
    }
}
