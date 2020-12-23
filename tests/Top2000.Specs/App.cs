using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            

            var baseAddress = new Uri("https://chrtop2000sadevwe.z6.web.core.windows.net/");

            services
                .AddFeatures()
                .AddClientDatabase(new DirectoryInfo(Directory.GetCurrentDirectory()), baseAddress);
        }
    }
}
