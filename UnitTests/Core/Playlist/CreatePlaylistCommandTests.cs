using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Interfaces;
using Core.Playlists.CreatePlaylist;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;


namespace UnitTests.Core.Playlist
{
    [TestFixture]
    [Category(Category.Playlist)]
    public class CreatePlaylistCommandTests
    {
        private static CreatePlaylistCommand.Handler GetHandler(PlaylistManagerDbContext context)
        {
            return new CreatePlaylistCommand.Handler(context);
        }

        [Test]
        public async Task CreatePlaylist_PlaylistCreated_ReturnsSuccessfully()
        {
            const string dbName = "PlaylistCreateReturnsSuccessful";
            using (var context = Utils.GetDbContext(dbName))
            {
                // arrange
                const int userId = 1;
                context.Users.Add(new User { Id = userId });
                var command = new CreatePlaylistCommand { Name = "Test Playlist", UserId = userId };

                // act
                var handler = GetHandler(context);
                await handler.Handle(command, CancellationToken.None);
            }

            using (var context = Utils.GetDbContext(dbName))
            {
                var count = context.Playlists.Count();
                Assert.AreEqual(1, count);
            }
        }

        [Test]
        public void CreatePlaylist_InvalidUser_Throws()
        {
            const string dbName = "PlaylistCreateThrows";
            using (var context = Utils.GetDbContext(dbName))
            {
                var command = new CreatePlaylistCommand {Name = "Test Playlist", UserId = 1};

                var handler = GetHandler(context);
                Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
            }
        }
    }
}
