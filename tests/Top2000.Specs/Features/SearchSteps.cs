using System.Collections.ObjectModel;
using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.Specs.Features;

[Binding]
public class SearchSteps
{
    private readonly IGroup grouping = new GroupByNothing();
    private readonly ISort sorting = new SortByTitle();
    private ReadOnlyCollection<IGrouping<string, Track>> result;
    private int lastEdition = 0;

    [Given(@"that last edition is (.*)")]
    public void GivenThatLastEditionIs(int lastEdition)
    {
        this.lastEdition = lastEdition;
    }

    [When(@"searching for (.*)")]
    public async Task WhenSearchingFor(string queryString)
    {
        var mediator = App.ServiceProvider.GetService<IMediator>();
        var request = new SearchTrackRequest
        {
            QueryString = queryString,
            Sorting = sorting,
            Grouping = grouping,
            LastEdition = lastEdition
        };

        result = await mediator.Send(request);
    }

    [When(@"searching without a query")]
    public async Task WhenSearchingWithoutAQuery()
    {
        var mediator = App.ServiceProvider.GetService<IMediator>();
        var request = new SearchTrackRequest
        {
            QueryString = string.Empty,
            Sorting = sorting,
            Grouping = grouping,
            LastEdition = lastEdition
        };

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

    public class TrackForAssertion
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;

        public int RecordedYear { get; set; }
    }
}