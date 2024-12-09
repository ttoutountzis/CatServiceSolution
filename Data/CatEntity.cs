using System.ComponentModel.DataAnnotations;

namespace CatServiceAPI.Data
{
    /// <summary>
    /// Represents a cat entity stored in the database.
    /// </summary>
    public class CatEntity
    {
        /// <summary>
        /// Auto-incremented unique ID for the cat.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique ID of the cat image from the Cat API.
        /// </summary>
        public string CatId { get; set; }

        /// <summary>
        /// The width of the cat image.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the cat image.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The binary data of the cat image.
        /// </summary>
        [Required]
        public byte[] Image { get; set; }

        /// <summary>
        /// Timestamp of when the record was created.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property for related tags.
        /// </summary>
        public ICollection<CatTag> CatTags { get; set; }
    }
}
