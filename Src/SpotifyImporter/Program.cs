﻿using System;
using System.IO;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpotifyImporter.Services;

namespace SpotifyImporter
{
    class Program
    {
        private static IConfigurationRoot _config;
        public static IConfigurationRoot Configuration { get; set; }

        public static async Task Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrWhiteSpace(env))
            {
                env = "Development";
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env == "Development")
            {
                builder.AddUserSecrets<Program>();
            }
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();

            _config = builder.Build();

            serviceCollection.Configure<AppSettings>(_config.GetSection("AppSettings"));

            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            await serviceProvider.GetService<App>().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddDebug();
            });
            
            services.AddDbContext<PlaylistManagerDbContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

            services.AddHttpClient();
            services.AddTransient<IApiService, ApiService>();

            services.AddTransient<App>();
        }
    }
}
