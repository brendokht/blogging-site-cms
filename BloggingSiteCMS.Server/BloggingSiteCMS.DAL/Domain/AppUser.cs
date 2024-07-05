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
        /// The first name of the user. This is required.
        /// </summary>
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// The last name of the user. This is optional.
        /// </summary>
        [StringLength(50)]
        [Required]
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// The date and time the user was created.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// The date and time the user was last modified.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}