using Chroomsoft.Top2000.Data;
using Chroomsoft.Top2000.Data.ClientDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            //TheProcess = Process.Start(@"dotnet run 'S:\github\Top2000\src\data\Data.StaticApiGenerator\Data.StaticApiGenerator.csproj'");

            //// var location = Path.Combine("wwwroot");

            // //await Task.WhenAll
            // //(
            // //    fileCreator.CreateApiFileAsync(location),
            // //    fileCreator.CreateDataFilesAsync(location)
            // //);

            var hostBuilder = new HostBuilder()
                .ConfigureServices(ConfigureServices)
            //    .ConfigureLogging(ConfigureLogging)
                .Build();

            ServiceProvider = hostBuilder.Services;

            // var fileCreator = ServiceProvider.GetService<IFileCreator>();
            // var currentDirectory = Directory.GetCurrentDirectory();
            // var location = Path.Combine(currentDirectory, "wwwroot");
            // fileCreator.CreateApiFileAsync(location).Wait();
            // fileCreator.CreateDataFilesAsync(location).Wait();
            // hostBuilder.StartAsync().Wait();
            // //Task.Run(() => hostBuilder.Run(), cancelSource.Token);
            // //
            // //hostBuilder.StartAsync(cancelSource.Token).GetAwaiter().GetResult();
            // //    hostBuilder.Run();
            // //      new Thread(new ThreadStart(() => { hostBuilder.Run(); })).Start();

            // ServiceProvider.GetService<ILogger<SpecsTest>>().LogInformation("BeforeTestRun setup completed");
        }

        private static void ConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            //Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Information()
            //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //.WriteTo.Seq("http://localhost:5341")
            //.CreateLogger();

            //loggingBuilder.AddSerilog();

            //loggingBuilder.AddConfiguration(new LoggerConfiguration());

            //loggingBuilder
            //    .ClearProviders()
            //    .WriteTo
            //    .AddConsole(console => { console.DisableColors = true; })
            //    ;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("top2000", c =>
            {
                c.BaseAddress = new Uri("https://www-dev.top2000.app");
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
