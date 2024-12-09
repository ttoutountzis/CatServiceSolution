namespace CatServiceAPI.Data
{
    /// <summary>
    /// Represents the many-to-many relationship between cats and tags.
    /// </summary>
    public class CatTag
    {
        public int CatId { get; set; }
        public CatEntity Cat { get; set; }
        public int TagId { get; set; }
        public TagEntity Tag { get; set; }
    }
}
