using Chroomsoft.Top2000.Data.ClientDatabase;
using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal static class Program
    {
        public static IServiceProvider ConfigureServices(AppRunner appRunner)
        {
#pragma warning disable S1075 // URIs should not be hardcoded
            var baseUrl = new Uri("https://www-dev.top2000.app");
#pragma warning restore S1075 // URIs should not be hardcoded

            var services = new ServiceCollection()
                .AddClientDatabase(new DirectoryInfo(Directory.GetCurrentDirectory()), baseUrl)
                .AddMediatR(Assembly.GetExecutingAssembly());

            foreach (Type type in appRunner.GetCommandClassTypes())
                services.AddTransient(type);

            return services.BuildServiceProvider();
        }

        private async static Task<int> Main(string[] args)
        {
            var appRunner = new AppRunner<App>();
            var serviceProvider = ConfigureServices(appRunner);

            var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "top2000data.db");

            if (!File.Exists(databasePath))
            {
                var updater = serviceProvider.GetRequiredService<IUpdateClientDatabase>();
                var localSource = serviceProvider.GetRequiredService<Top2000AssemblyDataSource>();
                await updater.RunAsync(localSource).ConfigureAwait(false);
            }

            return await appRunner
                .UseMicrosoftDependencyInjection(serviceProvider)
                .RunAsync(args).ConfigureAwait(false)
            ;
        }
    }
}
