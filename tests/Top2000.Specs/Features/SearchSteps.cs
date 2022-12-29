using Chroomsoft.Top2000.Features.Groupings;
using Chroomsoft.Top2000.Features.Searching;
using Chroomsoft.Top2000.Features.Sortings;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Chroomsoft.Top2000.Specs.Features
{
    [Binding]
    public class SearchSteps
    {
        private ReadOnlyCollection<IGrouping<string, Track>> result;
        private IGroup grouping = new GroupByNothing();
        private ISort sorting = new SortByTitle();

        [When(@"searching for (.*)")]
        public async Task WhenSearchingFor(string queryString)
        {
            var mediator = App.ServiceProvider.GetService<IMediator>();
            var request = new SearchTrackRequest(queryString, sorting, grouping);

            result = await mediator.Send(request);
        }

        [When(@"searching without a query")]
        public async Task WhenSearchingWithoutAQuery()
        {
            var mediator = App.ServiceProvider.GetService<IMediator>();
            var request = new SearchTrackRequest(string.Empty, sorting, grouping);

            result = await mediator.Send(request);
        }

        [Then(@"the following tracks are found:")]
        public void ThenTracksAreFoundWithTheFollowingDetails(Table table)
        {
            var expected = table
                .CreateSet<Track>()
                .Select(x => new TrackForAssertion
                {
                    Id = x.Id,
                    Title = x.Title,
                    Artist = x.Artist,
                    RecordedYear = x.RecordedYear,
                })
                .ToList();

            var actual = result
                .SelectMany(x => x)
                .Select(x => new TrackForAssertion
                {
                    Id = x.Id,
                    Title = x.Title,
                    Artist = x.Artist,
                    RecordedYear = x.RecordedYear,
                })
                .ToList();

            actual.Should().BeEquivalentTo(expected);
        }

        public class TrackForAssertion
        {
            public int Id { get; set; }

            public string Title { get; set; } = string.Empty;

            public string Artist { get; set; } = string.Empty;

            public int RecordedYear { get; set; }
        }

        [Then(@"the track (.*) is not found")]
        public void ThenTrackIsNotFound(string title)
        {
            var actual = result
                .SelectMany(x => x)
                .Where(x => x.Title == title);

            actual.Should().BeEmpty();
        }

        [Then(@"the results contain (.*) items")]
        public void ThenTheResultsContainItems(int count)
        {
            var actual = result
                .SelectMany(x => x);

            actual.Should()
                .HaveCount(count);
        }
    }
}
