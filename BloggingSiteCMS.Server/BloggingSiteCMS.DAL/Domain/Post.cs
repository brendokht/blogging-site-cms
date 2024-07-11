using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingSiteCMS.DAL.Domain
{
    public class Post : CMSEntity
    {
        /// <summary>
        /// The title of the post. This is required.
        /// </summary>
        [Required]
        public string? Title { get; set; }
        /// <summary>
        /// The Id of the AppUser (the Author).
        /// </summary>
        [Required]
        public string? AppUserId { get; set; }
        /// <summary>
        /// The summary of the post. This is optional.
        /// </summary>
        public string? Summary { get; set; }
        /// <summary>
        /// The content of the post. This is required.
        /// </summary>
        [Required]
        public string? Content { get; set; }
        /// <summary>
        /// The publish date of the post. This differs from the CreatedDate property, as this represents the date the post was published.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime PublishedDate { get; set; }
        /// <summary>
        /// A boolean value that represents whether the post is published or not.
        /// </summary>
        public bool IsPublished { get; set; } = false;

        [ForeignKey("AppUserId")]
        public virtual AppUser Author { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    }
}
