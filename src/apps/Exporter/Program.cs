using Chroomsoft.Top2000.Data;
using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.TrackInformation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1305 // Specify IFormatProvider
#pragma warning disable CS8604 // Possible null reference argument.

namespace Exporter
{
    internal class Program
    {
        private static async Task Main()
        {
            var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "top2000data.db");
            var connection = new SQLiteAsyncConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache, storeDateTimeAsTicks: false);

            var services = new ServiceCollection()
                .AddTransient<OnlineDataSource>()
                .AddTransient<Top2000AssemblyDataSource>()
                .AddTransient<IUpdateClientDatabase, UpdateDatabase>()
                .AddTransient<ITop2000AssemblyData, Top2000Data>()
                .AddTransient<SQLiteAsyncConnection>(f => connection)
                .AddMediatR(typeof(TrackInformationRequest).Assembly)
                .BuildServiceProvider();

            if (!File.Exists(databasePath))
            {
                var updater = services.GetService<IUpdateClientDatabase>();
                var localSource = services.GetService<Top2000AssemblyDataSource>();
                await updater.RunAsync(localSource).ConfigureAwait(false);
            }

            var mediator = services.GetService<IMediator>();
            var editions = new List<string>()
            {
                "Year"
            };
            var tracks = new List<string>()
            {
                "Id,Title,Artist,Year,HighPosition,HighEdition,LowPosition,LowEdition,FirstPosition,FirstEdition,LastPosition,LastEdition,LastPlayTime,Appearances,AppearancesPositions"
            };
            var possList = new List<string>
            {
                "Edition,Position,TrackId,Offset,OffsetType"
            };
            var watch = Stopwatch.StartNew();
            Console.WriteLine("Starting");

            var allEditions = (await mediator.Send(new AllEditionsRequest()).ConfigureAwait(false))
                .OrderBy(x => x.Year);
            foreach (var edition in allEditions)
            {
                editions.Add(edition.Year.ToString());
            }

            for (int i = 1; i < 4518; i++)
            {
                var trackInfo = new TrackInformationRequest(i);
                var track = await mediator.Send(trackInfo).ConfigureAwait(false);

                var strings = new List<string>
                {
                    i.ToString(),
                    Replace(track.Title),
                    Replace(track.Artist),
                    track.RecordedYear.ToString(),
                    track.Highest.Position.ToString(),
                    track.Highest.Edition.ToString(),
                    track.Lowest.Position.ToString(),
                    track.Lowest.Edition.ToString(),
                    track.First.Position.ToString(),
                    track.First.Edition.ToString(),
                    track.Latest.Position.ToString(),
                    track.Latest.Edition.ToString(),
                    track.Latest.PlayUtcDateAndTime.HasValue ? track.Latest.PlayUtcDateAndTime.Value.ToLocalTime().ToString("dd-MM-yyyy HH:mm") + $"-{track.Latest.PlayUtcDateAndTime.Value.ToLocalTime().Hour+1}:00" : "-",
                    track.Appearances.ToString(),
                    track.AppearancesPossible.ToString()
                }.ToArray();

                var listings = track.Listings
                    .Where(x => x.Status != ListingStatus.NotAvailable && x.Status != ListingStatus.NotListed && x.Status != ListingStatus.Unknown)
                    .OrderBy(x => x.Edition);

                foreach (var listing in listings)
                {
                    var listingString = new List<int>
                    {
                        listing.Edition,
                        listing.Position.Value,
                        i,
                        ReadOffSet(listing.Offset),
                        ToChr(listing.Status)
                    }.ToArray();
                    possList.Add(string.Join(',', listingString));
                }

                tracks.Add(string.Join(',', strings));
            }

            Console.WriteLine("Saving");
            await File.WriteAllLinesAsync("editions.csv", editions, Encoding.UTF8).ConfigureAwait(false);
            await File.WriteAllLinesAsync("tracks.csv", tracks, Encoding.UTF8).ConfigureAwait(false);
            await File.WriteAllLinesAsync("listing.csv", possList, Encoding.UTF8).ConfigureAwait(false);
            watch.Stop();
            Console.WriteLine($"Done in {watch.ElapsedMilliseconds}ms ({watch.Elapsed.TotalSeconds}s");
        }

        private static int ReadOffSet(int? value)
        {
            if (!value.HasValue)
                return 0;

            if (value.Value < 0)
                return value.Value * -1;

            return value.Value;
        }

        private static int ToChr(ListingStatus status)
        {
            return status switch
            {
                ListingStatus.New => 14,
                ListingStatus.Decreased => 31,
                ListingStatus.Increased => 30,
                ListingStatus.Unchanged => 61,
                ListingStatus.Back => 27,
                _ => throw new Exception(),
            };
        }

        private static string Replace(string input)
        {
#pragma warning disable CA1307 // Specify StringComparison
            return input
                .Replace("ä", "a")
                .Replace("á", "a")
                .Replace("à", "a")
                .Replace("Ø", "O")
                .Replace("ë", "e")
                .Replace("é", "e")
                .Replace("è", "e")
                .Replace("ö", "o")
                .Replace("ó", "o")
                .Replace("ò", "o")
                .Replace("ï", "i")
                .Replace("í", "i")
                .Replace("ì", "i")
                .Replace("&", "+")
                .Replace(",", " ")
                .Replace("ê", "e")
                .Replace("ã", "a")
                .Replace("õ", "o")
                .Replace("â", "a")
                .Replace("î", "i")
                .Replace("ø", "o")
                ;
        }
    }
}

#pragma warning restore CA1305 // Specify IFormatProvider
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CA1307 // Specify StringComparison
