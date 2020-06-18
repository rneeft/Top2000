using Chroomsoft.Top2000.Data;
using Chroomsoft.Top2000.Data.ClientDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;
using System;
using System.IO;
using TechTalk.SpecFlow;

[assembly: TestCategory("SkipWhenLiveUnitTesting")]

namespace Chroomsoft.Top2000.Specs
{
    [Binding]
    public sealed class App
    {
        public static string DatabasePath = Path.Combine(Directory.GetCurrentDirectory(), "top2000data.db");

        public static IServiceProvider ServiceProvider { get; set; } = new HostBuilder().Build().Services;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .Build();

            ServiceProvider = hostBuilder.Services;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("top2000", c =>
            {
                c.BaseAddress = new Uri("https://chrtop2000sadevwe.z6.web.core.windows.net/");
            });

            services
                .AddTransient<ITop2000AssemblyData, Top2000Data>()
                .AddTransient<OnlineDataSource>()
                .AddTransient<Top2000AssemblyDataSource>()
                .AddTransient<IUpdateClientDatabase, UpdateDatabase>()
                .AddTransient<ITop2000AssemblyData, Top2000Data>()
                .AddTransient<SQLiteAsyncConnection>(f =>
                {
                    return new SQLiteAsyncConnection(DatabasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                });
            ;
        }
    }
}
