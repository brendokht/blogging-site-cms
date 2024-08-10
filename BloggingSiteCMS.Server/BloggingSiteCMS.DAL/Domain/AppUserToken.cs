using System;

using Microsoft.AspNetCore.Identity;

namespace BloggingSiteCMS.DAL.Domain
{
    public class AppUserToken : IdentityUserToken<string>
    {
        public virtual AppUser? User { get; set; }
    }
}