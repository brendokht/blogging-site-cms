using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingSiteCMS.DAL.Domain
{
    /// <summary>
    /// Base class for all entities in the CMS.
    /// </summary>
    public class CMSEntity
    {
        /// <summary>
        /// The unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = new Guid().ToString();
        // Must assign new ConcurrencyStamp whenever persisting a new change to the model.
        /// <summary>
        /// A random value that must change whenever a user is persisted to the store, for concurrency reasons.
        /// </summary>
        [ConcurrencyCheck]
        public string ConcurrencyStamp { get; set; } = new Guid().ToString();
        /// <summary>
        /// The date and time the entity was created.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// The date and time the entity was last modified.
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
