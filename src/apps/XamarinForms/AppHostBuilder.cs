using Chroomsoft.Top2000.Apps.Common.Behavior;
using Chroomsoft.Top2000.Apps.Globalisation;
using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Essentials;

namespace Chroomsoft.Top2000.Apps
{
    public static class AppHostBuilder
    {
        public static IServiceProvider CreateServices(Action<HostBuilderContext, IServiceCollection> PlatformServices)
        {
            return new HostBuilder()
               .ConfigureHostConfiguration(ConfigureConfiguration)
               .ConfigureServices(ConfigureServices)
               .ConfigureServices(PlatformServices)
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
                .AddSingleton<NavigationShell.View>()
                .AddTransient<Overview.Position.ViewModel>()
                .AddTransient<Overview.Date.ViewModel>()
                .AddTransient<TrackInformation.ViewModel>()
                .AddSingleton<ICulture>(new SupportedCulture("nl"))
                .AddSingleton<ICulture>(new SupportedCulture("en"))
                .AddSingleton<ICulture>(new SupportedCulture("fr"))
            ;
        }

        private static void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.AddConsole(o => o.DisableColors = true);
        }

        private static string SaveAppSettingsToLocalDisk()
        {
            var filename = "Chroomsoft.Top2000.Apps.appsettings.json";
            var assembly = Assembly.GetExecutingAssembly();

            using var resFilestream = assembly.GetManifestResourceStream(filename)
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
