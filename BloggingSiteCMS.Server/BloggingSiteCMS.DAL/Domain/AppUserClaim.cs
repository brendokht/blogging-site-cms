using System;

using Microsoft.AspNetCore.Identity;

namespace BloggingSiteCMS.DAL.Domain
{
    public class AppUserClaim : IdentityUserClaim<string>
    {
        public virtual AppUser? User { get; set; }
    }
}