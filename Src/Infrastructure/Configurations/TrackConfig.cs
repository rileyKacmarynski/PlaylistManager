using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TrackConfig : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.SpotifyId).HasMaxLength(50);
            builder.Property(e => e.Name).HasMaxLength(200);

            builder.Property(e => e.Id)
                .UseSqlServerIdentityColumn()
                .HasColumnName("TrackId");

            builder.HasMany(t => t.PlaylistTracks)
                .WithOne(pt => pt.Track)
                .HasConstraintName("FK_Track_PlaylistTrack");
        }
    }
}