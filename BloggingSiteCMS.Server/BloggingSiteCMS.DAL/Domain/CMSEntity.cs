using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace BloggingSiteCMS.DAL.Domain
{
    /// <summary>
    /// Base class for all entities in the CMS.
    /// </summary>
    public abstract class CMSEntity
    {
        /// <summary>
        /// The unique identifier for the entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        /// <summary>
        /// A byte array that is automatically assigned a new value on update. Used for concurrency
        /// </summary>
        [Timestamp]
        public byte[]? Version { get; set; }
        /// <summary>
        /// The date and time the entity was created.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// The date and time the entity was last modified.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedAt { get; set; }
    }
}
