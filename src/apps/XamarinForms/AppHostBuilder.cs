using Chroomsoft.Top2000.Apps.AskForReview;
using Chroomsoft.Top2000.Apps.Common.Behavior;
using Chroomsoft.Top2000.Apps.Globalisation;
using Chroomsoft.Top2000.Apps.NavigationShell;
using Chroomsoft.Top2000.Apps.Themes;
using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;

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
               .Build().Services;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var baseUrl = new Uri("https://www-dev.top2000.app");

            services
                .AddClientDatabase(new DirectoryInfo(FileSystem.AppDataDirectory), baseUrl)
                .AddFeatures()
                .AddSingleton<IThemeService, ThemeService>()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddTransient<Overview.Position.ViewModel>()
                .AddTransient<Overview.Date.ViewModel>()
                .AddTransient<TrackInformation.ViewModel>()
                .AddTransient<Searching.ViewModel>()
                .AddTransient<IAskForReview, ReviewModule>()
                .AddSingleton<IPreferences, PreferencesImplementation>()
                .AddSingleton<ICulture>(new SupportedCulture("nl"))
                .AddSingleton<ICulture>(new SupportedCulture("en"))
                .AddSingleton<ICulture>(new SupportedCulture("fr"))
            ;

            if (IsTop2000Live())
            {
                services.AddSingleton<IMainShell, NavigationShell.LiveTop2000.View>();
            }
            else
            {
                services.AddSingleton<IMainShell, NavigationShell.View>();
            }

            services.Configure<AskForReviewConfiguration>(context.Configuration.GetSection(nameof(AskForReviewConfiguration)));
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

        private static bool IsTop2000Live()
        {
            var current = DateTime.UtcNow;

            var first = new DateTime(current.Year, 12, 24, 23, 0, 0, DateTimeKind.Utc); // first day of Christmas for CET in UTC time
            var last = new DateTime(current.Year, 12, 31, 23, 0, 0, DateTimeKind.Utc); // new year for CET in UTC time

            return (current > first && current < last);
        }
    }
}
