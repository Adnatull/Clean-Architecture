using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Identity.Entities;

namespace Core.Domain.Identity.Constants
{
    public class DefaultApplicationUsers
    {
        public static ApplicationUser GetSuperUser()
        {
            var defaultUser = new ApplicationUser
            {
                Id = "39c64ad6-39ae-4de0-a0b2-9298a46b4b4c",
                UserName = "SuperAdmin",
                Email = "a2masum@yahoo.com",
                FirstName = "Al",
                LastName = "Masum",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            return defaultUser;
        }
    }
}
