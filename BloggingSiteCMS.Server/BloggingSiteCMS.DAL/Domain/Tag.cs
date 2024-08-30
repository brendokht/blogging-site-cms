using System.ComponentModel.DataAnnotations;

namespace BloggingSiteCMS.DAL.Domain
{
    public class Tag : CMSEntity
    {
        /// <summary>
        /// The tag's name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }
    }
}