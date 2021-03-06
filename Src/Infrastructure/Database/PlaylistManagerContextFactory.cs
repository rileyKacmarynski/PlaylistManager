﻿using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    // This idea is shamelessly stolen from https://github.com/JasonGT/NorthwindTraders/blob/master/Northwind.Persistence/NorthwindDbContextFactory.cs
    public class PlaylistManagerContextFactory : DesignTimeDbContextFactoryBase<PlaylistManagerDbContext>
    {
        protected override PlaylistManagerDbContext CreateNewInstance(DbContextOptions<PlaylistManagerDbContext> options)
        {
            return new PlaylistManagerDbContext(options);
        }
    }
}
