using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingSiteCMS.DAL.Domain
{
    public class Comment : CMSEntity
    {
        /// <summary>
        /// The content of the post. This is required.
        /// </summary>
        [Required]
        public string? Content { get; set; }
        /// <summary>
        /// The Id of the AppUser (the Commenter).
        /// </summary>
        [Required]
        public string? AppUserId { get; set; }
        /// <summary>
        /// The Id of the Post
        /// </summary>
        [Required]
        public string? PostId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser Author { get; set; } = null!;
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; } = null!;
    }
}
