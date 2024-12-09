namespace CatServiceAPI.Data
{
    /// <summary>
    /// Represents a tag entity that describes a cat's temperament.
    /// </summary>
    public class TagEntity
    {
        /// <summary>
        /// Auto-incremented unique ID for the tag.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the tag (e.g., temperament).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Timestamp of when the record was created.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property for related cats.
        /// </summary>
        public ICollection<CatTag> CatTags { get; set; }
    }
}
