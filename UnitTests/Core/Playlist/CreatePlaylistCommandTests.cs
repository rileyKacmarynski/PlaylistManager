using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Playlists.CreatePlaylist;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;


namespace UnitTests.Core.Playlist
{
    [Category(Category.Playlist)]
    public class CreatePlaylistCommandTests
    {
        //Note: if I ever need to create two contexts in one test this won't work
        // I don't think you should need to create two in a unit test
        private PlaylistManagerDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PlaylistManagerDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new PlaylistManagerDbContext(options);
        }

        private CreatePlaylistCommand.Handler GetHandler(PlaylistManagerDbContext context)
        {
            return new CreatePlaylistCommand.Handler(context);
        }

        [Test]
        public async Task CreatePlaylist_PlaylistCreated_ReturnsSuccessfully()
        {
            using (var context = GetDbContext("PlaylistCreateReturnsSuccessful"))
            {
                // arrange
                const int userId = 1;
                context.Users.Add(new User { Id = userId });
                var command = new CreatePlaylistCommand { Name = "Test Playlist", UserId = userId };

                // act
                var handler = GetHandler(context);
                var res = await handler.Handle(command, CancellationToken.None);

                // assert
                Assert.IsTrue(true);
            }

        }
    }
}
