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
    internal class Program
    {
        public static IServiceProvider ConfigureServices(AppRunner appRunner)
        {
            var services = new ServiceCollection()
                .AddClientDatabase(new DirectoryInfo(Directory.GetCurrentDirectory()))
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
                var updater = serviceProvider.GetService<IUpdateClientDatabase>();
                var localSource = serviceProvider.GetService<Top2000AssemblyDataSource>();
                await updater.RunAsync(localSource).ConfigureAwait(false);
            }

            return await appRunner
                .UseMicrosoftDependencyInjection(serviceProvider)
                .RunAsync(args).ConfigureAwait(false);
            ;
        }
    }
}
