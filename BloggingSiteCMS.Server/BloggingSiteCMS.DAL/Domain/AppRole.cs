using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace BloggingSiteCMS.DAL.Domain
{
    public class AppRole : IdentityRole<string>
    {
        [Required]
        public string? Description { get; set; }

        public virtual ICollection<AppUserRole>? UserRoles { get; set; }
        public virtual ICollection<AppRoleClaim>? RoleClaims { get; set; }
    }
}