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
        public string? FirstName { get; set; }
        /// <summary>
        /// The last name of the user.
        /// </summary>
        [StringLength(50)]
        public string? LastName { get; set; }
        /// <summary>
        /// Bio of the user.
        /// </summary>
        [StringLength(100)]
        public string? Bio { get; set; }
        /// <summary>
        /// The date and time the user was created.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// The date and time the user was last modified.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedAt { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<AppUserClaim>? Claims { get; set; } = new List<AppUserClaim>();
        public virtual ICollection<AppUserLogin>? Logins { get; set; } = new List<AppUserLogin>();
        public virtual ICollection<AppUserToken>? Tokens { get; set; } = new List<AppUserToken>();
        public virtual ICollection<AppUserRole>? UserRoles { get; set; } = new List<AppUserRole>();

    }
}