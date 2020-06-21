using Chroomsoft.Top2000.Data;
using Chroomsoft.Top2000.Data.ClientDatabase;
using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
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
        public EditionsCommand Editions { get; set; }
    }

    [Command(Name = "Editions")]
    public class EditionsCommand
    {
        private readonly IMediator mediator;
        private readonly IUpdateClientDatabase updateClientDatabase;
        private readonly Top2000AssemblyDataSource assemblyDataSource;

        public EditionsCommand(IMediator mediator, IUpdateClientDatabase updateClientDatabase, Top2000AssemblyDataSource assemblyDataSource)
        {
            this.mediator = mediator;
            this.updateClientDatabase = updateClientDatabase;
            this.assemblyDataSource = assemblyDataSource;
        }

        [SubCommand]
        public ListCommand List { get; set; } = null!;

        [DefaultMethod]
        public async Task All()
        {
            var editions = await mediator.Send(new AllEditionsRequest());

            foreach (var edition in editions)
            {
                Console.WriteLine(edition.Year);
            }
        }
    }

    [Command(Name = "List")]
    public class ListCommand
    {
        [DefaultMethod]
        public void List(int year)
        {
            if (year < 0 || year > 2019)
                Console.WriteLine("Wrong year");
            else
            {
                Console.WriteLine("Bohemian Rhapsody");
                Console.WriteLine("Hotel California");
            }
        }
    }

    public class AllEditionsRequest : IRequest<ImmutableSortedSet<Edition>>
    {
    }

    public class AllEditionsRequestHandler : IRequestHandler<AllEditionsRequest, ImmutableSortedSet<Edition>>
    {
        private readonly SQLiteAsyncConnection connection;

        public AllEditionsRequestHandler(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<ImmutableSortedSet<Edition>> Handle(AllEditionsRequest request, CancellationToken cancellationToken)
        {
            return (await connection.Table<Edition>().ToListAsync().ConfigureAwait(false))
                .ToImmutableSortedSet(new EditionComparer());
        }
    }

    public class EditionComparer : IComparer<Edition>
    {
        public int Compare(Edition x, Edition y)
        {
            return x.Year.CompareTo(y.Year);
        }
    }

    public class Edition
    {
        public int Year { get; set; }

        public DateTimeOffset StartDateAndTime { get; set; }

        public DateTimeOffset EndDateAndTime { get; set; }
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

            if (File.Exists(databasePath))
                File.Delete(databasePath);

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

        private static Task<int> Main(string[] args)
        {
            var appRunner = new AppRunner<App>();
            // await db creation.

            return appRunner
                .UseMicrosoftDependencyInjection(ConfigureServices(appRunner))
                .RunAsync(args);
            ;
        }
    }
}
