using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.SpotifyId).HasMaxLength(50);
            builder.Property(e => e.Name).HasMaxLength(200);
            builder.Property(e => e.ReleaseDate).HasMaxLength(100);

            builder.Property(e => e.Id)
                .UseSqlServerIdentityColumn()
                .HasColumnName("AlbumId");

            builder.HasOne(a => a.Artist)
                .WithMany(a => a.Albums)
                .HasConstraintName("FK_Artist_Album");
        }
    }
}