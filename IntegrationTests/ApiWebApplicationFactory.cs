using System;
using System.Collections.Generic;
using System.Text;
using API;
using Infrastructure.Database;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationTests
{
    public class ApiWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a dat(abase context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<PlaylistManagerDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemorySharedDb");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<PlaylistManagerDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<ApiWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        DbUtils.SeedDatabaseForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error occured seeding database with test data: {ex.Message}");
                    }
                }
            });
        }

    }
}
