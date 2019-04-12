
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Configurations
{
    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(e => e.Id);
                
            builder.Property(e => e.Id)
                .UseSqlServerIdentityColumn()
                .HasColumnName("PlaylistId");

            builder.Property(e => e.SpotifyId).HasMaxLength(50);
            builder.Property(e => e.Name).HasMaxLength(200);

            builder.HasOne(p => p.User)
                .WithMany(u => u.Playlists)
                .HasConstraintName("FK_User_Playlist");

            builder.HasMany(p => p.PlaylistTracks)
                .WithOne(pt => pt.Playlist)
                .HasConstraintName("FK_Playlist_PlaylistTrack");
        }
    }
}
