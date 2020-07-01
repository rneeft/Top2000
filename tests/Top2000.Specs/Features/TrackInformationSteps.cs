using Chroomsoft.Top2000.Features;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Chroomsoft.Top2000.Specs.Features
{
    [Binding]
    public class TrackInformationSteps
    {
        private TrackInformation track;

        [When(@"the track information feature is executed for TrackId (.*)")]
        public async Task WhenTheTrackInformationFeatureIsExecutedForTrackId(int trackId)
        {
            var request = new TrackInformationRequest(trackId);

            var mediator = App.ServiceProvider.GetService<IMediator>();

            track = await mediator.Send(request);
        }

        [Then(@"the title is '(.*)'")]
        public void ThenTheTitleIs(string title)
        {
            track.Title.Should().Be(title);
        }

        [Then(@"the artist is '(.*)'")]
        public void ThenTheArtistIs(string artist)
        {
            track.Artist.Should().Be(artist);
        }

        [Then(@"the year is (.*)")]
        public void ThenTheYearIs(int year)
        {
            track.Year.Should().Be(year);
        }

        [Then(@"the set with appearances contains all the editions")]
        public void ThenTheSetWithAppearancesContainsAllTheEditions()
        {
            var mediator = App.ServiceProvider.GetService<IMediator>();
            var alleditions
        }
    }
}
