using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Chroomsoft.Top2000.Specs.Features
{
    [Binding]
    public class AllListingsOfEditionSteps
    {
        private ImmutableHashSet<TrackListing> result;

        [Then(@"an empty set is returned")]
        public void ThenAnEmptySetIsReturned()
        {
            result.Should().BeEmpty();
        }

        [When(@"the AllListingOfEdition feature is executed with year (.*)")]
        public async Task WhenTheAllListingOfEditionFeatureIsExecutedWithYear(int year)
        {
            var request = new AllListingsOfEditionRequest(year);
            var mediator = App.ServiceProvider.GetService<IMediator>();

            result = await mediator.Send(request);
        }
    }
}
