using Lumina.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lumina.Data
{
    public class LuminaContext : DbContext
    {
        public LuminaContext(DbContextOptions<LuminaContext> options)
            : base(options) { }

        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<CollageEntity> Collages { get; set; }
        public DbSet<CollageImageEntity> CollageImages { get; set; }
        public DbSet<ImageLayerEntity> ImageLayers { get; set; }
        public DbSet<EffectEntity> Effects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CollageImageEntity>()
                .HasKey(ci => new { ci.CollageId, ci.ImageId });

            modelBuilder.Entity<CollageImageEntity>()
                .HasOne(ci => ci.Collage)
                .WithMany(c => c.CollageImages)
                .HasForeignKey(ci => ci.CollageId);

            modelBuilder.Entity<CollageImageEntity>()
                .HasOne(ci => ci.Image)
                .WithMany(i => i.CollageImages)
                .HasForeignKey(ci => ci.ImageId);

            modelBuilder.Entity<ImageLayerEntity>()
                .HasOne(l => l.Collage)
                .WithMany(c => c.Layers)
                .HasForeignKey(l => l.CollageId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
