using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.Logging;

namespace SpotifyImporter
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrWhiteSpace(env))
            {
                env = "Development";
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, true)
                .AddJsonFile($"appsettings{env}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env == "Development")
            {
                builder.AddUserSecrets<Program>();
            }
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(builder.Build().GetSection("AppSettings"));

            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddDebug();
            });

            serviceCollection.AddTransient<App>();
        }
    }
}
