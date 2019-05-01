using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Interfaces;
using Core.Playlists.AddSongsToPlaylist;
using Domain;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace UnitTests.Core.Playlists
{
    [Category(Category.Playlist)]
    public class AddSongsToPlaylistCommandTests
    {
        private static AddSongsToPlaylistCommand.Handler GetCommandHandler(IPlaylistManagerDbContext dbContext)
        {
            return new AddSongsToPlaylistCommand.Handler(dbContext);
        }

        // add songs from list to playlist
        [Test]
        public async Task AddSongsToPlaylist_ContainsSongIdList_AddsSongsToPlaylist()
        {
            const string dbName = "AddToPlaylistContainsSongs";
            const int id = 1;

            // Arrange
            using (var context = Utils.GetDbContext(dbName))
            {
                context.Playlists.Add(new Playlist {Id = id});
                context.Tracks.AddRange(new List<Track>
                {
                    new Track {Id = 1},
                    new Track {Id = 2}
                });
                context.SaveChanges();
            }

            // Act
            using (var context = Utils.GetDbContext(dbName))
            {
                var command = new AddSongsToPlaylistCommand
                {
                    PlaylistId = 1,
                    TrackIds = new List<int> { 1, 2 }
                };

                var handler = GetCommandHandler(context);
                await handler.Handle(command, CancellationToken.None);

                var tracks = context.Playlists
                    .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.Track)
                    .SelectMany(p => p.PlaylistTracks.Select(pt => pt.Track));

                // Assert
                Assert.AreEqual(2, tracks.Count());
            }
        }

        // add albums from list to playlist
        [Test]
        public async Task AddSongsToPlaylist_ContainsAlbumIdList_AddsSongsToPlaylist()
        {
            const string dbName = "AddToPlaylistContainsAlbums";
            const int playlistId = 1;
            const int albumId = 2;

            // Arrange
            using (var context = Utils.GetDbContext(dbName))
            {
                context.Playlists.Add(new Playlist { Id = playlistId });
                var album = new Album { Id = albumId };
                context.Albums.Add(album);
                context.Tracks.AddRange(new List<Track>
                {
                    new Track {Id = 1, Album = album},
                    new Track {Id = 2, Album = album}
                });
   
                context.SaveChanges();
            }

            // Act
            using (var context = Utils.GetDbContext(dbName))
            {
                var command = new AddSongsToPlaylistCommand
                {
                    PlaylistId = 1,
                    AlbumIds = new List<int> { albumId }
                };

                
                var handler = GetCommandHandler(context);
                await handler.Handle(command, CancellationToken.None);

                var tracks = context.Playlists
                    .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.Track)
                    .SelectMany(p => p.PlaylistTracks.Select(pt => pt.Track));

                // Assert
                Assert.AreEqual(2, tracks.Count());
            }
        }


        // add albums from list to playlist
        [Test]
        public void AddSongsToPlaylist_AlbumNotFound_ThrowsNotFoundError()
        {
            const string dbName = "AddToPlaylistAlbumNotFound";
            const int playlistId = 1;
            
            // Arrange
            using (var context = Utils.GetDbContext(dbName))
            {
                context.Playlists.Add(new Playlist { Id = playlistId });
                context.Albums.AddRange(new List<Album>
                {
                    new Album{Id = 2}
                });
                context.SaveChanges();
            }

            using (var context = Utils.GetDbContext(dbName))
            {
                var command = new AddSongsToPlaylistCommand
                {
                    PlaylistId = 1,
                    AlbumIds = new List<int> { 1 }
                };

                // Act
                var handler = GetCommandHandler(context);
                
                // Assert
                Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
            }
        }

        // return not found if playlist cannot be found
        [Test]
        public void AddSongsToPlaylist_PlaylistNotFound_ThrowsNotFoundError()
        {
            const string dbName = "AddToPlaylistPlaylistNotFound";

            // Arrange
            using (var context = Utils.GetDbContext(dbName))
            {
                context.Albums.AddRange(new List<Album>
                {
                    new Album{Id = 2}
                });
                context.SaveChanges();
            }

            using (var context = Utils.GetDbContext(dbName))
            {
                var command = new AddSongsToPlaylistCommand
                {
                    PlaylistId = 1,
                    AlbumIds = new List<int> { 2 }
                };

                // Act
                var handler = GetCommandHandler(context);
                
                // Assert
                Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
            }
        }

        [Test]
        public void AddSongsToPlaylist_SongsNotFound_ThrowsNotFoundError()
        {
            const string dbName = "AddToPlaylistSongsNotFound";
            const int playlistId = 1;

            // Arrange
            using (var context = Utils.GetDbContext(dbName))
            {
                context.Playlists.Add(new Playlist { Id = playlistId });
                context.Tracks.AddRange(new List<Track>
                {
                    new Track{Id = 1}
                });
                context.SaveChanges();
            }

            using (var context = Utils.GetDbContext(dbName))
            {
                var command = new AddSongsToPlaylistCommand
                {
                    PlaylistId = 1,
                    TrackIds = new List<int> { 2 }
                };

                // Act
                var handler = GetCommandHandler(context);

                // Assert
                Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
            }
        }
    }
}
