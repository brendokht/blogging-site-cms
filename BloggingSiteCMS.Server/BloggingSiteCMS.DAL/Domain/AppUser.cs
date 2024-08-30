using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace BloggingSiteCMS.DAL.Domain
{
    /// <summary>
    /// Represents a user in the application.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// The first name of the user.
        /// </summary>
        [StringLength(50)]
        [PersonalData]
        public string? FirstName { get; set; }
        /// <summary>
        /// The last name of the user.
        /// </summary>
        [StringLength(50)]
        [PersonalData]
        public string? LastName { get; set; }
        /// <summary>
        /// Bio of the user.
        /// </summary>
        [StringLength(256)]
        [PersonalData]
        public string? Bio { get; set; }
        /// <summary>
        /// The date and time the user was created.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        [PersonalData]
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// The date and time the user was last modified.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        [PersonalData]
        public DateTime ModifiedAt { get; set; }

        public virtual List<Comment>? Comments { get; set; }
        public virtual ICollection<AppUserClaim>? Claims { get; set; }
        public virtual ICollection<AppUserLogin>? Logins { get; set; }
        public virtual ICollection<AppUserToken>? Tokens { get; set; }
        public virtual ICollection<AppUserRole>? UserRoles { get; set; }

    }
}