using System;

using Microsoft.AspNetCore.Identity;

namespace BloggingSiteCMS.DAL.Domain
{
    public class AppUserLogin : IdentityUserLogin<string>
    {
        public virtual AppUser? User { get; set; }
    }
}