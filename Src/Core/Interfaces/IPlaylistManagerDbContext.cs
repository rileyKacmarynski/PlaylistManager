using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Interfaces
{
    public interface IPlaylistManagerDbContext
    {
        DbSet<Playlist> Playlists { get; set; }
        DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        DbSet<Track> Tracks { get; set; }
        DbSet<Artist> Artists { get; set; }
        DbSet<Album> Albums { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
