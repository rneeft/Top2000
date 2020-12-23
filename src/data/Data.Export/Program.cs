using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features;
using Chroomsoft.Top2000.Features.AllEditions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Export
{
    internal class Program
    {
        private static IServiceProvider? serviceProvider;

        public static IServiceProvider ServiceProvider
        {
            get
            {
                return serviceProvider ??
                    throw new InvalidOperationException("Application isn't booted yet");
            }
            set
            {
                serviceProvider = value;
            }
        }

        public static T GetService<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();

        public static Task EnsureDatabaseIsCreatedAsync()
        {
            var databaseGen = GetService<IUpdateClientDatabase>();
            var top2000 = GetService<Top2000AssemblyDataSource>();

            return databaseGen.RunAsync(top2000);
        }

        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var services = new ServiceCollection()
                .AddFeatures()
                .AddOfflineClientDatabase(new DirectoryInfo(Directory.GetCurrentDirectory()))
                .AddLogging(configure => configure.AddConsole())
                .AddSingleton<IConfiguration>(configuration)
                ;

            serviceProvider = services.BuildServiceProvider();

            await EnsureDatabaseIsCreatedAsync();

            var tracks = new List<string>()
            {
                "Id,Title,Artist,Year,HighPosition,HighEdition,LowPosition,LowEdition,FirstPosition,FirstEdition,LastPosition,LastEdition,LastPlayTime,Appearances,AppearancesPositions"
            };
            var possList = new List<string>
            {
                "Edition,Position,TrackId,Offset,OffsetType"
            };

            var mediator = GetService<IMediator>();

            var editions = new List<string>() { "Year" };
            var allEditions = (await mediator.Send(new AllEditionsRequest()))
                .OrderBy(x => x.Year)
                .Select(x => $"{x.Year}")
                .ToList();

            editions.AddRange(allEditions);
        }
    }
}
