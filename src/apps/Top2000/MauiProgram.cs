using Chroomsoft.Top2000.Apps.Globalisation;
using Microsoft.Extensions.Logging;
using Chroomsoft.Top2000.Apps.AskForReview;
using Chroomsoft.Top2000.Apps.NavigationShell;
using Chroomsoft.Top2000.Apps.Themes;
using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features;

namespace Top2000
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<Chroomsoft.Top2000.Apps.XamarinForms.App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("MaterialIcons.ttf", "MaterialIcons");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<ILocalisationService, LocalisationService>();
            // services.AddTransient<IStoreReview, StoreReviewImplementation>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var baseUrl = new Uri("https://www-dev.top2000.app");

            builder.Services
                .AddClientDatabase(new DirectoryInfo(FileSystem.Current.AppDataDirectory), baseUrl)
                .AddFeatures()
                .AddSingleton<IThemeService, ThemeService>()
                .AddTransient<Chroomsoft.Top2000.Apps.Overview.Position.ViewModel>()
                .AddTransient<Chroomsoft.Top2000.Apps.Overview.Date.ViewModel>()
                .AddTransient<Chroomsoft.Top2000.Apps.TrackInformation.ViewModel>()
                .AddTransient<Chroomsoft.Top2000.Apps.Searching.ViewModel>()
                .AddTransient<IAskForReview, ReviewModule>()
                // .AddSingleton<Xamarin.Essentials.Interfaces.IPreferences, Xamarin.Essentials.Implementation.PreferencesImplementation>()
                .AddSingleton<ICulture>(new SupportedCulture("nl"))
                .AddSingleton<ICulture>(new SupportedCulture("en"))
                .AddSingleton<ICulture>(new SupportedCulture("fr"))
            ;

            if (IsTop2000Live())
            {
                builder.Services.AddSingleton<IMainShell, Chroomsoft.Top2000.Apps.NavigationShell.LiveTop2000.View>();
            }
            else
            {
                builder.Services.AddSingleton<IMainShell, Chroomsoft.Top2000.Apps.NavigationShell.View>();
            }

            // builder.Services.Configure<AskForReviewConfiguration>(builder.Configuration.GetSection(nameof(AskForReviewConfiguration)));




            var serviceProvider = builder.Build();

            Chroomsoft.Top2000.Apps.XamarinForms.App.ServiceProvider = serviceProvider.Services;
            Chroomsoft.Top2000.Apps.XamarinForms.App.EnsureDatabaseIsCreatedAsync().GetAwaiter().GetResult();


            return serviceProvider;
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
