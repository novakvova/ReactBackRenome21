using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Shop.Data.Entities.Identity
{
    public class AppUser : IdentityUser<long>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
