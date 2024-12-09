using Microsoft.EntityFrameworkCore;

namespace CatServiceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CatEntity> Cats { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<CatTag> CatTags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatTag>()
                .HasKey(ct => new { ct.CatId, ct.TagId });

            modelBuilder.Entity<CatTag>()
                .HasOne(ct => ct.Cat)
                .WithMany(c => c.CatTags)
                .HasForeignKey(ct => ct.CatId);

            modelBuilder.Entity<CatTag>()
                .HasOne(ct => ct.Tag)
                .WithMany(t => t.CatTags)
                .HasForeignKey(ct => ct.TagId);
        }
    }
}
