using Chroomsoft.Top2000.Data.ClientDatabase;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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

        [Then(@"except for the last year, the position table contains 2000 tracks for each year ranging from 1 to 2000")]
        public async Task ThenThePositionTableContainsTracksForEachYearRangingFromTo()
        {
            var sql = App.ServiceProvider.GetService<SQLiteAsyncConnection>();
            var lists = (await sql.Table<PositionTable>().ToListAsync())
                .GroupBy(x => x.Year)
                .OrderBy(x => x.Key)
                .ToList();

            var expected = Enumerable.Range(1, 2000);

            for (int i = 0; i < lists.Count - 2; i++)
            {
                var yearPositions = lists[i].Select(x => x.Position).OrderBy(x => x);
                yearPositions.Should().BeEquivalentTo(expected);
            }
        }

        [Then(@"except for the last year, the playlist table contains 2000 tracks for each year after 2016")]
        public async Task ThenThePlaylistTableContainsTracksForEachYearAfterRangingFromTo()
        {
            var sql = App.ServiceProvider.GetService<SQLiteAsyncConnection>();
            var lists = (await sql.Table<PlayTime>().ToListAsync())
                .GroupBy(x => x.Year)
                .OrderBy(x => x.Key)
                .ToList();

            for (int i = 0; i < lists.Count - 2; i++)
            {
                lists[i].Count().Should().Be(2000);
            }
        }

        [Then(@"the positions table contains 10 or 2000 for the last year ranging from 1 to 10/2000")]
        public async Task ThenThePositionsTableContainsOrForTheLastYearRangingFromTo()
        {
            var sql = App.ServiceProvider.GetService<SQLiteAsyncConnection>();
            var list = (await sql.Table<PlayTime>().ToListAsync())
                .GroupBy(x => x.Year)
                .OrderBy(x => x.Key)
                .Last();

            list.Count().Should().BeOneOf(10, 2000);
        }

        [Then(@"the playlist table contains 10 or 2000 tracks for the last year")]
        public void ThenThePlaylistTableContainsOrTracksForTheLastYearRangingFromTo()
        {
            // when picking up listing as entity this will be filled
        }

        [Then(@"for each track in the playlist table the playtime is the same to the previous track or has incremented by one hour")]
        public void ThenForEachTrackInThePlaylistTableThePlaytimeIsTheSameToThePreviousTrackOrHasIncrementByOneHour()
        {
            // when picking up listing as entity this will be filled
        }

        private class PlayTime
        {
            public int TrackId { get; set; }

            public int Year { get; set; }

            public DateTimeOffset DateAndTime { get; set; }
        }

        [Table("Position")]
        private class PositionTable
        {
            public int TrackId { get; set; }

            public int Year { get; set; }

            public int Position { get; set; }
        }
    }
}
