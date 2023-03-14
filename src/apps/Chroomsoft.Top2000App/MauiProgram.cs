using Chroomsoft.Top2000App.Pages;
using Microsoft.Maui.LifecycleEvents;

namespace Chroomsoft.Top2000App;

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
            });
        builder.ConfigureLifecycleEvents(lifecycle => {
#if WINDOWS
            lifecycle.AddWindows(windows => windows.OnWindowCreated((del) => {
                del.ExtendsContentIntoTitleBar = true;
            }));
#endif
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var services = builder.Services;

#pragma warning disable S1075 // URIs should not be hardcoded
        var baseUrl = new Uri("https://www-dev.top2000.app");
#pragma warning restore S1075 // URIs should not be hardcoded

        services
            .AddClientDatabase(new DirectoryInfo(FileSystem.AppDataDirectory), baseUrl)
            .AddFeatures()
            .AddTransient<PositionsOverviewViewModel>()
            .AddTransient<PositionsOverviewView>()
            ;

        var app =  builder.Build();

#if WINDOWS
        SQLitePCL.Batteries.Init();
            SQLitePCL.raw.sqlite3_win32_set_directory(1, FileSystem.AppDataDirectory);
            SQLitePCL.raw.sqlite3_win32_set_directory(2, FileSystem.CacheDirectory);
#endif
     

        return app;
    }



}

public class MyWindow : Window
{
    private readonly IUpdateClientDatabase databaseGenerator;
    private readonly Top2000AssemblyDataSource assemblyDataSource;

    public MyWindow(IUpdateClientDatabase databaseGenerator, Top2000AssemblyDataSource assemblyDataSource)
    {
        this.databaseGenerator = databaseGenerator;
        this.assemblyDataSource = assemblyDataSource;
    }

    protected override async void OnCreated()
    {
        await databaseGenerator.RunAsync(assemblyDataSource);
    }
}