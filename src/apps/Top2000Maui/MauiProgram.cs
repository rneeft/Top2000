namespace Chroomsoft.Top2000.Apps;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons.ttf", "MaterialIcons");
            })
            ;

        builder.Services
            .AddSingleton<ILocalisationService, LocalisationService>()
            .AddSingleton<ICulture>(new SupportedCulture("nl"))
            .AddSingleton<ICulture>(new SupportedCulture("en"))
            .AddSingleton<ICulture>(new SupportedCulture("fr"));

        var baseUrl = new Uri("https://www-dev.top2000.app");
        builder.Services.AddClientDatabase(new DirectoryInfo(FileSystem.AppDataDirectory), baseUrl);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}