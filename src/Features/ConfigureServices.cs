namespace Chroomsoft.Top2000.Features;

public static class ConfigureServices
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        return services
            .AddTransient<IAllEditions, AllEditions>()
            .AddTransient<IAllListingsOfEdition, AllListingsOfEdition>()
            .AddTransient<ICalculateTrackDetails, TrackDetailsCalculator>()
            .AddTransient<ISearch, Search>()

            .AddSingleton<ISortSearch, SortByTitle>()
            .AddSingleton<ISortSearch, SortByArtist>()
            .AddSingleton<ISortSearch, SortByRecordedYear>()
            .AddSingleton<IGroupSearch, GroupByNothing>()
            .AddSingleton<IGroupSearch, GroupByArtist>()
            .AddSingleton<IGroupSearch, GroupByRecordedYear>()
            ;
    }
}