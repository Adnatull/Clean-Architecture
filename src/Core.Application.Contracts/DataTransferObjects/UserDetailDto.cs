using System.ComponentModel.DataAnnotations;

namespace Core.Application.Contracts.DataTransferObjects
{
    public class UserDetailDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
