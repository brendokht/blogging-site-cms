using Microsoft.AspNetCore.Identity;

namespace BloggingSiteCMS.DAL.DomainClasses
{
    /// <summary>
    /// Represents a user in the application.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// The first name of the user. This is required.
        /// </summary>
        public required string FirstName { get; set; }
        /// <summary>
        /// The last name of the user. This is optional.
        /// </summary>
        public string? LastName { get; set; }
    }
}