using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing;

namespace Core.Playlists.AddSongsToPlaylist
{
    public class AddSongsToPlaylistCommand : IRequest
    {
        public AddSongsToPlaylistCommand()
        {
            TrackIds = new List<int>();
            AlbumIds = new List<int>();
        }

        public int PlaylistId { get; set; }
        public IEnumerable<int> TrackIds { get; set; }
        public IEnumerable<int> AlbumIds { get; set; }
        public class Handler : IRequestHandler<AddSongsToPlaylistCommand, Unit>
        {
            private readonly IPlaylistManagerDbContext _dbContext;

            public Handler(IPlaylistManagerDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(AddSongsToPlaylistCommand command, CancellationToken cancellationToken)
            {
                var playlist = await _dbContext.Playlists.FindAsync(command.PlaylistId);
                if (playlist == null) throw new NotFoundException("Playlist", command.PlaylistId);

                CheckThatAlbumsExist(command);
                CheckThatSongsExist(command);

                var tracks = _dbContext.Tracks.Include(t => t.Album)
                    .Where(t => command.TrackIds.Contains(t.Id) || command.AlbumIds.Contains(t.Album.Id));

                await tracks.ForEachAsync(t =>
                        _dbContext.PlaylistTracks.Add(new PlaylistTrack { Track = t, Playlist = playlist }),
                    cancellationToken);

                playlist.TotalTracks = playlist.PlaylistTracks.Count;

                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            private void CheckThatSongsExist(AddSongsToPlaylistCommand command)
            {
                if (!command.TrackIds.Any()) return;

                var tracksFound = _dbContext.Tracks.Where(a => command.TrackIds.Contains(a.Id));
                var tracksMissing = command.TrackIds.Except(tracksFound.Select(a => a.Id));

                if (tracksMissing.Any()) throw new NotFoundException("Tracks", tracksMissing);
            }

            private void CheckThatAlbumsExist(AddSongsToPlaylistCommand command)
            {
                if (!command.AlbumIds.Any()) return;

                var albumsFound = _dbContext.Albums.Where(a => command.AlbumIds.Contains(a.Id));
                var albumsMissing = command.AlbumIds.Except(albumsFound.Select(a => a.Id));

                if (albumsMissing.Any()) throw new NotFoundException("Albums", albumsMissing);
            }
        }
    }
}
