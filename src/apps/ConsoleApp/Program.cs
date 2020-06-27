using Chroomsoft.Top2000.Data;
using Chroomsoft.Top2000.Data.ClientDatabase;
using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class App
    {
        public App()
        {
            Console.WriteLine("Starting");
            Console.ReadKey();
        }

        [SubCommand]
        public EditionsCommand Editions { get; set; } = null!;
    }

    [Command(Name = "Editions")]
    public class EditionsCommand
    {
        private readonly IMediator mediator;

        public EditionsCommand(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [SubCommand]
        public ListCommand List { get; set; } = null!;

        [DefaultMethod]
        public async Task All()
        {
            //var editions = await mediator.Send(new AllEditionsRequest());

            //foreach (var edition in editions)
            //{
            //    Console.WriteLine(edition.Year);
            //}

            throw new NotImplementedException();
        }
    }

    [Command(Name = "List")]
    public class ListCommand
    {
        private readonly IMediator mediator;

        public ListCommand(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [DefaultMethod]
        public async Task List(int year)
        {
            var tracks = (await mediator.Send(new AllListingsOfEditionRequest(year)).ConfigureAwait(false))
                .OrderBy(x => x.Position)
                .ToList();

            foreach (var track in tracks)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("{0,4}", track.Position);
                Console.Write(" ");
                Console.WriteLine(track.Title);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"     {track.Artist}");

                Console.ResetColor();
            }
        }
    }

    public class AllListingsOfEditionRequest : IRequest<List<TrackListing>>
    {
        public AllListingsOfEditionRequest(int year)
        {
            Year = year;
        }

        public int Year { get; }
    }

    public class AllListingsOfEditionRequestHandler : IRequestHandler<AllListingsOfEditionRequest, List<TrackListing>>
    {
        private readonly SQLiteAsyncConnection connection;

        public AllListingsOfEditionRequestHandler(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<List<TrackListing>> Handle(AllListingsOfEditionRequest request, CancellationToken cancellationToken)
        {
            var sql =
                "SELECT Listing.TrackId, Position, PlayDateAndTime, Title, Artist " +
                "FROM Listing JOIN Track ON TrackId = Id " +
                $"WHERE Edition = ?";

            return (await connection.QueryAsync<TrackListing>(sql, request.Year));
        }
    }

    public class TrackListing
    {
        public int TrackId { get; set; }

        public int Position { get; set; }

        public DateTimeOffset PlayDateAndTime { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;
    }

    internal class Program
    {
        public static IServiceProvider ConfigureServices(AppRunner appRunner)
        {
            var services = new ServiceCollection()
                .AddMediatR(Assembly.GetExecutingAssembly());

            ConfigureClientDatabase(services);

            foreach (Type type in appRunner.GetCommandClassTypes())
                services.AddTransient(type);

            return services.BuildServiceProvider();
        }

        public static IServiceCollection ConfigureClientDatabase(IServiceCollection services)
        {
            services.AddHttpClient("top2000", c =>
            {
                c.BaseAddress = new Uri("https://www-dev.top2000.app");
            });

            var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "top2000data.db");

            return services
                .AddTransient<OnlineDataSource>()
                .AddTransient<Top2000AssemblyDataSource>()
                .AddTransient<IUpdateClientDatabase, UpdateDatabase>()
                .AddTransient<ITop2000AssemblyData, Top2000Data>()
                .AddSingleton<SQLiteAsyncConnection>(f =>
                {
                    return new SQLiteAsyncConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                });
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
