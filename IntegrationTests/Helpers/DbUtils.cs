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
            context.Users.Add(new User
            {
                Id = 1,
                DisplayName = "Test",
                Email = "Test@domain.com"
            });

            context.SaveChanges();
        }
    }
}
