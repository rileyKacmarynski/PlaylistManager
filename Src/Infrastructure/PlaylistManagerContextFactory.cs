using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure
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
