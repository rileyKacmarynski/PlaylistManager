using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ArtistsConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.SpotifyId).HasMaxLength(50);
            builder.Property(e => e.Name).HasMaxLength(200);

            builder.Property(e => e.Id)
                .UseSqlServerIdentityColumn()
                .HasColumnName("ArtistId");
        }
    }
}