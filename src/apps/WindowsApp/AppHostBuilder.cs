using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features;
using Chroomsoft.Top2000.WindowsApp.Common;
using Chroomsoft.Top2000.WindowsApp.Common.Behavior;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Essentials;

namespace Chroomsoft.Top2000.WindowsApp
{
    public static class AppHostBuilder
    {
        public static IServiceProvider CreateServices()
        {
            return new HostBuilder()
               .ConfigureHostConfiguration(ConfigureConfiguration)
               .ConfigureServices(ConfigureServices)
               .ConfigureLogging(ConfigureLogging)
               .Build().Services;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var baseUrl = new Uri("https://www-dev.top2000.app");

            services
                .AddClientDatabase(new DirectoryInfo(FileSystem.AppDataDirectory), baseUrl)
                .AddFeatures()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddTransient<Navigation.View>()
                .AddTransient<YearOverview.View>()
                .AddSingleton<YearOverview.ViewModel>()
                .AddSingleton<ListingDate.ViewModel>()
                .AddSingleton<ListingPosition.ViewModel>()
                .AddSingleton<TrackInformation.ViewModel>()
                .AddSingleton<Searching.ViewModel>()
                .AddSingleton<About.ViewModel>()
                .AddSingleton<About.View>()
                .AddSingleton<IGlobalUpdate, GlobalUpdates>()
                .AddTransient<IOnlineUpdateChecker, OnlineUpdateChecker>()
            ;
        }

        private static void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.AddConsole(o => o.DisableColors = true);
        }

        private static string SaveAppSettingsToLocalDisk()
        {
            var filename = "appsettings.json";
            var assembly = Assembly.GetExecutingAssembly();
            var appsettingsFile = assembly.GetName().Name + "." + filename;

            using var resFilestream = assembly.GetManifestResourceStream(appsettingsFile)
                ?? throw new FileNotFoundException($"Unable to find {filename} in {assembly.GetName()}");

            var localPath = Path.Combine(FileSystem.CacheDirectory, filename);

            using var stream = File.Create(localPath);
            resFilestream.CopyTo(stream);

            return localPath;
        }

        private static void ConfigureConfiguration(IConfigurationBuilder builder)
        {
            var fullConfig = SaveAppSettingsToLocalDisk();

            builder.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
            builder.AddJsonFile(fullConfig);
        }
    }
}
