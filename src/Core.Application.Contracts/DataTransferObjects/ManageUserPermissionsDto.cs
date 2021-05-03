using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.Contracts.DataTransferObjects
{
    public class ManageUserPermissionsDto
    {
        [Required]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<ManageClaimDto> ManagePermissionsDto { get; set; }
    }

    
}
