using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Infrastructure.Database;

namespace IntegrationTests.Helpers
{
    public class DbUtils
    {
        public static void SeedDatabaseForTests(PlaylistManagerDbContext context)
        {
            var user1 = new User
            {
                DisplayName = "Test",
                Email = "Test@domain.com"
            };
            context.Users.Add(user1);

            context.Playlists.Add(new Playlist
            {
                Name = "Test Playlist 1",
                TotalTracks = 0,
                User = user1
            });

            context.SaveChanges();
        }
    }
}
