using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Playlists.GetPlaylists;
using Domain;
using Infrastructure.Database;
using NUnit.Framework;

namespace UnitTests.Core.Playlists
{
    [Category(Category.Playlist)]
    public class GetPlaylistsQueryTests
    {
        private static GetPlaylistsQuery.Handler GetQueryHandler(PlaylistManagerDbContext context)
        {
            return new GetPlaylistsQuery.Handler(context);
        }

        [Test]
        public async Task GetPlaylists_GivenUserId_ReturnsPlaylistsForUser()
        {
            const string dbName = "GetPlaylistWithUserId";
            using (var context = Utils.GetDbContext(dbName))
            {
                // arrange
                const int userId = 1;
                var user = new User {Id = userId};
                context.Users.Add(user);
                context.Playlists.Add(new Domain.Playlist {Id = 1, User = user, Name = "Test"});
                context.SaveChanges();

                var query = new GetPlaylistsQuery {UserId = userId};

                // act
                var handler = GetQueryHandler(context);
                var playlists = await handler.Handle(query, CancellationToken.None);

                // assert
                Assert.AreEqual(1, playlists.Count());
                Assert.AreEqual(userId, playlists.First().User.Id);
            }
        }

        [Test]
        public async Task GetPlaylists_GivenNonExistentUserId_ReturnsEmptyList()
        {
            const string dbName = "GetPlaylistWithMissingId";
            using (var context = Utils.GetDbContext(dbName))
            {
                // arrange
                const int userId = 5;
                var user = new User { Id = userId };
                context.Users.Add(user);
                context.Playlists.Add(new Domain.Playlist { Id = 1, User = new User{Id = 1}, Name = "Test" });
                context.SaveChanges();

                var query = new GetPlaylistsQuery { UserId = userId };

                // act
                var handler = GetQueryHandler(context);
                var playlists = await handler.Handle(query, CancellationToken.None);

                // assert
                Assert.AreEqual(0, playlists.Count());
            }
        }

        [Test]
        public async Task GetPlaylists_GivenName_ReturnsPlaylistsByName()
        {
            const string dbName = "GetPlaylistWithName";
            using (var context = Utils.GetDbContext(dbName))
            {
                // arrange
                const int userId = 1;
                var user = new User { Id = userId };
                context.Users.Add(user);
                context.Playlists.Add(new Domain.Playlist { Id = 1, User = user, Name = "Test" });
                context.SaveChanges();

                var query = new GetPlaylistsQuery { Name =  "Test"};

                // act
                var handler = GetQueryHandler(context);
                var playlists = await handler.Handle(query, CancellationToken.None);

                // assert
                Assert.AreEqual(1, playlists.Count());
                Assert.AreEqual("Test", playlists.First().Name);
            }
        }

        [Test]
        public async Task GetPlaylists_GivenUserIdAndName_ReturnsPlaylistsForUserAndName()
        {

            const string dbName = "GetPlaylistWithBoth";
            using (var context = Utils.GetDbContext(dbName))
            {
                // arrange
                const int userId = 1;
                var user = new User { Id = userId };
                context.Users.Add(user);
                context.Playlists.Add(new Domain.Playlist { Id = 1, User = user, Name = "Test" });
                context.Playlists.Add(new Domain.Playlist { Id = 2, User = new User{Id = 2}, Name = "Something Different" });
                context.SaveChanges();

                var query = new GetPlaylistsQuery { Name = "Test" };

                // act
                var handler = GetQueryHandler(context);
                var playlists = await handler.Handle(query, CancellationToken.None);

                // assert
                Assert.AreEqual(1, playlists.Count());
                Assert.AreEqual("Test", playlists.First().Name);
                Assert.AreEqual(userId, playlists.First().User.Id);
            }
        }
    }
}
