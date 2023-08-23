﻿using Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;
using Chroomsoft.Top2000.Apps.Views.SelectEdition;
using Chroomsoft.Top2000.Apps.Views.TrackInformation;
using Chroomsoft.Top2000.Features;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;

namespace Chroomsoft.Top2000.Apps;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
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

        builder.Services
            .AddSingleton<EditionOnView>()
            .AddTransientWithShellRoute<OverviewByPositionPage, OverviewByPositionViewModel>(nameof(OverviewByPositionPage))
            .AddTransientWithShellRoute<TrackInformationPage, TrackInformationViewModel>(nameof(TrackInformationPage))
            .AddTransientWithShellRoute<SelectEditionsPage, SelectEditionsViewModel>(nameof(SelectEditionsPage))
            ;

        var baseUrl = new Uri("https://www-dev.top2000.app");
        builder.Services
            .AddClientDatabase(new DirectoryInfo(FileSystem.AppDataDirectory), baseUrl)
            .AddFeatures();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}