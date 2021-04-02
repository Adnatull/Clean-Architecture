using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CA.Core.Domain.Identity.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string name): base(name)
        {
            
        }
        public string Description { get; set; }
    }
}
