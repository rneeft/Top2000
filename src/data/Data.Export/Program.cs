using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features;
using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllTrackIds;
using Chroomsoft.Top2000.Features.TrackInformation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Export
{
    internal class Program
    {
        private static readonly Exception MustBeHereException = new NotSupportedException("Position should be available");

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

            var trackIds = (await mediator.Send(new AllTrackIdRequest()));

            foreach (var trackId in trackIds)
            {
                var trackInfo = new TrackInformationRequest(trackId);
                var track = await mediator.Send(trackInfo);

                var strings = new List<string>
                {
                    trackId.ToString(CultureInfo.InvariantCulture),
                    Replace(track.Title),
                    Replace(track.Artist),
                    track.RecordedYear.ToString(CultureInfo.InvariantCulture),
                    track.Highest.Position?.ToString(CultureInfo.InvariantCulture) ?? throw MustBeHereException,
                    track.Highest.Edition.ToString(CultureInfo.InvariantCulture),
                    track.Lowest.Position?.ToString(CultureInfo.InvariantCulture) ?? throw MustBeHereException,
                    track.Lowest.Edition.ToString(CultureInfo.InvariantCulture),
                    track.First.Position?.ToString(CultureInfo.InvariantCulture) ?? throw MustBeHereException,
                    track.First.Edition.ToString(CultureInfo.InvariantCulture),
                    track.Latest.Position?.ToString(CultureInfo.InvariantCulture) ?? throw MustBeHereException,
                    track.Latest.Edition.ToString(CultureInfo.InvariantCulture),
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
                        listing?.Position ?? throw MustBeHereException,
                        trackId,
                        ReadOffSet(listing.Offset),
                        ToChr(listing.Status)
                    }.ToArray();
                    possList.Add(string.Join(',', listingString));
                }

                tracks.Add(string.Join(',', strings));
            }

            Encoding utf8WithoutBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

            await File.WriteAllLinesAsync("editions.csv", editions, utf8WithoutBom).ConfigureAwait(false);
            await File.WriteAllLinesAsync("tracks.csv", tracks, utf8WithoutBom).ConfigureAwait(false);
            await File.WriteAllLinesAsync("listings.csv", possList, utf8WithoutBom).ConfigureAwait(false);
        }

        private static string Replace(string input)
        {
            return input
                .Replace("ä", "a", StringComparison.InvariantCulture)
                .Replace("á", "a", StringComparison.InvariantCulture)
                .Replace("à", "a", StringComparison.InvariantCulture)
                .Replace("ã", "a", StringComparison.InvariantCulture)
                .Replace("â", "a", StringComparison.InvariantCulture)
                .Replace("å", "a", StringComparison.InvariantCulture)
                
                .Replace("Å", "A", StringComparison.InvariantCulture)

                .Replace("ê", "e", StringComparison.InvariantCulture)
                .Replace("ë", "e", StringComparison.InvariantCulture)
                .Replace("é", "e", StringComparison.InvariantCulture)
                .Replace("è", "e", StringComparison.InvariantCulture)
                .Replace("È", "E", StringComparison.InvariantCulture)

                .Replace("ö", "o", StringComparison.InvariantCulture)
                .Replace("ó", "o", StringComparison.InvariantCulture)
                .Replace("ò", "o", StringComparison.InvariantCulture)
                .Replace("ô", "o", StringComparison.InvariantCulture)
                .Replace("õ", "o", StringComparison.InvariantCulture)

                .Replace("ø", "o", StringComparison.InvariantCulture)
                .Replace("Ø", "O", StringComparison.InvariantCulture)

                .Replace("î", "i", StringComparison.InvariantCulture)
                .Replace("ï", "i", StringComparison.InvariantCulture)
                .Replace("í", "i", StringComparison.InvariantCulture)
                .Replace("ì", "i", StringComparison.InvariantCulture)
                .Replace("î", "i", StringComparison.InvariantCulture)

                .Replace("&", "+", StringComparison.InvariantCulture)
                .Replace(",", " ", StringComparison.InvariantCulture)
                ;
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
    }
}
