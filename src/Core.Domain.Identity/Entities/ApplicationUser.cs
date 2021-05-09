using Microsoft.AspNetCore.Identity;

namespace Core.Domain.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}
