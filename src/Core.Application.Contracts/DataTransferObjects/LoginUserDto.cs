using System.ComponentModel.DataAnnotations;

namespace Core.Application.Contracts.DataTransferObjects
{
    public class LoginUserDto
    {
        [Required]
        [Display(Name = "UserName")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        public bool RememberMe { get; set; }
    }
}
