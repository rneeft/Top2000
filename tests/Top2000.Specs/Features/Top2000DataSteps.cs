using Chroomsoft.Top2000.Data.ClientDatabase;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Chroomsoft.Top2000.Specs.Features
{
    [Binding]
    public class Top2000DataSteps
    {
        [BeforeScenario]
        public void DeleteClientDatabase()
        {
            if (File.Exists(App.DatabasePath))
                File.Delete(App.DatabasePath);
        }

        [AfterScenario]
        public void CloseDatabaseConnections()
        {
            SQLite.SQLiteAsyncConnection.ResetPool();
        }

        [Given(@"all data scripts")]
        public void GivenAllDataScripts()
        {
        }

        [When(@"the client database is created")]
        public async Task WhenTheClientDatabaseIsCreatedAsync()
        {
            var assemblySource = App.ServiceProvider.GetService<Top2000AssemblyDataSource>();
            var update = App.ServiceProvider.GetService<IUpdateClientDatabase>();

            await update.RunAsync(assemblySource);
        }

        [Then(@"except for the last edition, the listing table contains 2000 tracks for each edition ranging from 1 to 2000")]
        public async Task ThenThePositionTableContainsTracksForEachEditionRangingFromTo()
        {
            var sql = App.ServiceProvider.GetService<SQLiteAsyncConnection>();
            var lists = (await sql.Table<Listing>().ToListAsync())
                .GroupBy(x => x.Edition)
                .OrderBy(x => x.Key)
                .ToList();

            var expected = Enumerable.Range(1, 2000);

            for (int i = 0; i < lists.Count - 2; i++)
            {
                var yearPositions = lists[i].Select(x => x.Position).OrderBy(x => x);
                yearPositions.Should().BeEquivalentTo(expected);
            }
        }

        [Then(@"the listing table contains 10 or 2000 for the last edition ranging from 1 to 10/2000")]
        public async Task ThenThePositionsTableContainsOrForTheLastYearRangingFromTo()
        {
            var sql = App.ServiceProvider.GetService<SQLiteAsyncConnection>();
            var list = (await sql.Table<Listing>().ToListAsync())
                .GroupBy(x => x.Edition)
                .OrderBy(x => x.Key)
                .Last();

            list.Count().Should().BeOneOf(10, 2000);
        }

        [Then(@"for each track in the listing table the PlayDateAndTime is the same to the previous track or has incremented by one hour")]
        public async Task ThenForEachTrackInTheListingTableThePlayDateAndTimeIsTheSameToThePreviousTrackOrHasIncrementedByOneHour()
        {
            var sql = App.ServiceProvider.GetService<SQLiteAsyncConnection>();
            var listings = (await sql.Table<Listing>().Where(x => x.Edition > 2015).ToListAsync())
                .OrderBy(x => x.Position)
                .GroupBy(x => x.Edition)
                .OrderBy(x => x.Key)
                .ToList();

            foreach (var listing in listings)
            {
                var previous = listing.First();

                foreach (var track in listing)
                {
                    var differenceInHours = previous.PlayUtcDateAndTime - track.PlayUtcDateAndTime;

                    Assert.IsTrue(differenceInHours.Value.TotalMinutes == 0 || differenceInHours.Value.TotalMinutes == 60,
                        $"For edition {listing.Key} the positions {previous.Position} and {track.Position} the PlayDateAndTime is incorrect, it is {differenceInHours.Value.TotalMinutes} and should be either 0 or 60"
                        );

                    previous = track;
                }
            }
        }

        private class Listing
        {
            public int TrackId { get; set; }

            public int Edition { get; set; }

            public int Position { get; set; }

            public DateTime? PlayUtcDateAndTime { get; set; }
        }
    }
}
