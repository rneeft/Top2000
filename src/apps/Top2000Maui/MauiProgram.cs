﻿using Chroomsoft.Top2000.Apps.Views.Overview.ByDate;
using Chroomsoft.Top2000.Apps.Views.Overview.ByDate.SelectDateTimeGroup;
using Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;
using Chroomsoft.Top2000.Apps.Views.Search;
using Chroomsoft.Top2000.Apps.Views.TrackInformation;
using Chroomsoft.Top2000.Features;
using CommunityToolkit.Maui;

namespace Chroomsoft.Top2000.Apps;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons.ttf", "MaterialIcons");
            });

#if ANDROID
        builder.ConfigureMauiHandlers((handlers) =>
        {
            handlers.AddHandler(typeof(Shell), typeof(Platforms.Android.MyShellRenderer));
        });
#endif

        builder.Services
            .AddSingleton<ILocalisationService, LocalisationService>()
            .AddSingleton<ICulture>(new SupportedCulture("nl"))
            //   .AddSingleton<ICulture>(new SupportedCulture("en"))
            // .AddSingleton<ICulture>(new SupportedCulture("fr"))
            ;

        builder.Services
            .AddTransientWithShellRoute<OverviewByPositionPage, OverviewByPositionViewModel>(nameof(OverviewByPositionPage))
            .AddTransientWithShellRoute<TrackInformationPage, TrackInformationViewModel>(nameof(TrackInformationPage))
            .AddTransientWithShellRoute<OverviewByDatePage, OverviewByDateViewModel>(nameof(OverviewByDatePage))
            .AddTransientWithShellRoute<SelectDateTimeGroupPage, SelectDateTimeGroupViewModel>(nameof(SelectDateTimeGroupPage))
            .AddTransientWithShellRoute<SearchPage, SearchViewModel>(nameof(SearchPage))
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