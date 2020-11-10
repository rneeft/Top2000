using Chroomsoft.Top2000.Features.Searching;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Chroomsoft.Top2000.Specs.Features
{
    [Binding]
    public class SearchSteps
    {
        private ImmutableHashSet<Track> result;

        [When(@"searching for (.*)")]
        public async Task WhenSearchingFor(string queryString)
        {
            var mediator = App.ServiceProvider.GetService<IMediator>();
            var request = new SearchTrackRequest(queryString);

            result = await mediator.Send(request);
        }

        [Then(@"the following tracks are found:")]
        public void ThenTracksAreFoundWithTheFollowingDetails(Table table)
        {
            var expected = table.CreateSet<Track>();
            result.ToList().Should().BeEquivalentTo(expected);
        }

        [Then(@"only the track (.*) by (.*) recorded in (.*) is found")]
        public void ThenOnlyOneTrackIsFound(string title, string artist, int recordedYear)
        {
            result.Count.Should().Be(1);
            var track = result.Single();

            using (new AssertionScope())
            {
                track.Title.Should().Be(title);
                track.Artist.Should().Be(artist);
                track.RecordedYear.Should().Be(recordedYear);
            }
        }

        [Then(@"the track (.*) is not found")]
        public void ThenTrackIsNotFound(string title)
        {
            result.Where(x => x.Title == title).Should().BeEmpty();
        }
    }
}
