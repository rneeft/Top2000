﻿using Chroomsoft.Top2000.Features.Favorites;
using Chroomsoft.Top2000.Features.Searching;
using Microsoft.Extensions.DependencyInjection;

namespace Chroomsoft.Top2000.Features
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            return services
                .AddMediatR(configuration =>
                {
                    configuration.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly);
                })
                .AddSingleton<ISort, SortByTitle>()
                .AddSingleton<ISort, SortByArtist>()
                .AddSingleton<ISort, SortByRecordedYear>()
                .AddSingleton<IGroup, GroupByNothing>()
                .AddSingleton<IGroup, GroupByArtist>()
                .AddSingleton<IGroup, GroupByRecordedYear>()
                .AddTransient<FavoritesHandler>()
                ;
        }
    }
}