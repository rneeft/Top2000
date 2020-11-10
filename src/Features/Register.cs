using Chroomsoft.Top2000.Features.Searching;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Chroomsoft.Top2000.Features
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(ConfigureServices).Assembly)
                .AddSingleton<SortByArtist>()
                .AddSingleton<SortByTitle>()
                .AddSingleton<SortByRecordedYear>()
                .AddSingleton<GroupByArtist>()
                .AddSingleton<GroupByNothing>()
                .AddSingleton<GroupByRecordedYear>()
                ;
        }
    }
}
