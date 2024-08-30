using System.ComponentModel.DataAnnotations;

namespace BloggingSiteCMS.DAL.Domain
{
    public class Category : CMSEntity
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(100)]
        public string? Description { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
    }
}