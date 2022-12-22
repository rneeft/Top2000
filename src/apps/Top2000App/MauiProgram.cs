using Chroomsoft.Top2000.Data.ClientDatabase;
using Microsoft.Extensions.Logging;

namespace Top2000App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterAppServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
        {
            var baseUrl = new Uri("https://www-dev.top2000.app");

            mauiAppBuilder.Services
                .AddClientDatabase(new DirectoryInfo(FileSystem.AppDataDirectory), baseUrl);

            return mauiAppBuilder;
        }
    }
}