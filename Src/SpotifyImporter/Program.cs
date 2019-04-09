using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SpotifyImporter
{
    class Program
    {
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
                .AddJsonFile("appsettings.json", optional: true, true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env == "Development")
            {
                builder.AddUserSecrets<Program>();
            }
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();

            var config = builder.Build();

            serviceCollection.Configure<AppSettings>(config.GetSection("AppSettings"));

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

            services.AddHttpClient();

            services.AddTransient<App>();
        }
    }
}
