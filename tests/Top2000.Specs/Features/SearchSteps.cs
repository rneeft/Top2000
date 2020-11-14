using Chroomsoft.Top2000.Features.Searching;
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

        [Then(@"the following tracks are found:")]
        public void ThenTracksAreFoundWithTheFollowingDetails(Table table)
        {
            var expected = table.CreateSet<Track>();
            var actual = result.SelectMany(x => x)
                .ToList();

            actual.Should().BeEquivalentTo(expected);
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

            actual.Should().NotBeEmpty()
                .And.HaveCount(count);
        }
    }
}
