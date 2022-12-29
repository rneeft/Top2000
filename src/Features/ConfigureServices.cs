using Chroomsoft.Top2000.Features.Groupings;
using Chroomsoft.Top2000.Features.Sortings;
using Chroomsoft.Top2000.Features.TrackInformation;
using Microsoft.Extensions.DependencyInjection;

namespace Chroomsoft.Top2000.Features
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            return services
                .AddTransient<IAllEditions, AllEditions>()
                .AddTransient<IAllListingsOfEdition, AllListingsOfEdition>()
                .AddTransient<ICalculateTrackDetails, TrackDetailsCalculator>()
                .AddTransient<IAllListingsOfEdition , AllListingsOfEdition>()
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
}