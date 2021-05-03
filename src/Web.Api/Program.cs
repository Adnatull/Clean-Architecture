using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Web.Framework.Extensions;

namespace Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1 && (args[0] == "--version" || args[0] == "-Version" || args[0] == "-V" || args[0] == "--v"))
            {
                GetVersionInformation();
                return;
            }
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            try
            {
                Log.Information("Application Starting.");
                var host = CreateHostBuilder(args)
                    .Build()
                    .MigrateAndSeed();
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void GetVersionInformation()
        {
            var runtimeVersion = typeof(Startup)
                .GetTypeInfo()
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
            Console.WriteLine("Clean Architecture Template: " + runtimeVersion);
            var copyright = typeof(Startup)
                .GetTypeInfo()
                .Assembly
                .GetCustomAttribute<AssemblyCopyrightAttribute>()
                ?.Copyright;
            Console.WriteLine("Copyright " + copyright);
        }
    }
}
