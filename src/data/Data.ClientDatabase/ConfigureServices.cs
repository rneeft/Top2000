using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System;
using System.IO;

namespace Chroomsoft.Top2000.Data.ClientDatabase
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddClientDatabase(this IServiceCollection services, DirectoryInfo appDataDirectory, string clientDatabaseName = "Top2000.db")
        {
            services.AddHttpClient("top2000", c =>
            {
                c.BaseAddress = new Uri("https://www-dev.top2000.app");
            });

            return services
                .AddTransient<OnlineDataSource>()
                .AddTransient<Top2000AssemblyDataSource>()
                .AddTransient<IUpdateClientDatabase, UpdateDatabase>()
                .AddTransient<ITop2000AssemblyData, Top2000Data>()
                .AddTransient<SQLiteAsyncConnection>(f =>
                {
                    var databasePath = Path.Combine(appDataDirectory.FullName, clientDatabaseName);

                    return new SQLiteAsyncConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache, storeDateTimeAsTicks: false);
                });
        }
    }
}
