
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

    public class PlaylistTrackConfig : IEntityTypeConfiguration<PlaylistTrack>
    {
        public void Configure(EntityTypeBuilder<PlaylistTrack> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseSqlServerIdentityColumn()
                .HasColumnName("PlaylistTrackId");
        }
    }

    public class TrackConfig : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.SpotifyId).HasMaxLength(50);
            builder.Property(e => e.Name).HasMaxLength(200);

            builder.Property(e => e.Id)
                .UseSqlServerIdentityColumn()
                .HasColumnName("UserId");

            builder.HasMany(t => t.PlaylistTracks)
                .WithOne(pt => pt.Track)
                .HasConstraintName("FK_Track_PlaylistTrack");
        }
    }
}
