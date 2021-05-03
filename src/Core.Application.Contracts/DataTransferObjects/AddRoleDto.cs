using System.ComponentModel.DataAnnotations;

namespace Core.Application.Contracts.DataTransferObjects
{
    public class AddRoleDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
