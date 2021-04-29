using System.ComponentModel.DataAnnotations;

namespace CA.Core.Application.Contracts.DataTransferObjects
{
    public class AddRoleDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
