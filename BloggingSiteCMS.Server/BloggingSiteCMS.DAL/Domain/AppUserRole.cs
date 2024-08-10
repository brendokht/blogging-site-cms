using System;

using Microsoft.AspNetCore.Identity;

namespace BloggingSiteCMS.DAL.Domain
{
    public class AppUserRole : IdentityUserRole<string>
    {
        public virtual AppUser? User { get; set; }
        public virtual AppRole? Role { get; set; }
    }
}