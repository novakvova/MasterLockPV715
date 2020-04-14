using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterLock.Entities
{
    public class DbUser : IdentityUser<long>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Age { get; set; }
        public string Date { get; set; }
       // public string StringRole { get; set; }
        public string Url { get; set; }
        public ICollection<DbUserRole> UserRoles { get; set; }
    }
}
