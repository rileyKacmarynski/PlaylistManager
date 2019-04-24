using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public class Utils
    {
        // Since I'm checking against the in memory datastore is this technically an 
        // integration test? Maybe... But it's easier than creating a fake context. 
        public static PlaylistManagerDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PlaylistManagerDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new PlaylistManagerDbContext(options);
        }
    }
}
