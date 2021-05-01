using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CA.Core.Application.Contracts.DataTransferObjects
{
    public class ManageUserRolesDto
    {
        [Required]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<ManageRolesDto> ManageRolesDtos { get; set; }
    }
    public class ManageRolesDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Checked { get; set; }
    }
}
