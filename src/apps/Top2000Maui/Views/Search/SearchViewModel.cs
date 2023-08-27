using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.Apps.Views.Search;

public sealed partial class SearchViewModel : ObservableObject
{
    private readonly IMediator mediator;
    private readonly IEnumerable<IGroup> groupOptions;
    private readonly IEnumerable<ISort> sortOptions;

    [ObservableProperty]
    private string queryText = string.Empty;

    [ObservableProperty]
    private GroupViewModel groupBy = new(new GroupByNothing(), "Niet");

    [ObservableProperty]
    private SortViewModel sortBy = new(new SortByTitle(), "Titel");

    [ObservableProperty]
    private string resultCount = string.Empty;

    public SearchViewModel(IMediator mediator, IEnumerable<IGroup> groupOptions, IEnumerable<ISort> sortOptions)
    {
        this.mediator = mediator;
        this.groupOptions = groupOptions;
        this.sortOptions = sortOptions;
        this.Results = new();
        this.GroupOptions = new();
        this.SortOptions = new();
    }

    public ObservableRangeCollection<GroupViewModel> GroupOptions { get; }

    public ObservableRangeCollection<SortViewModel> SortOptions { get; }
    public ObservableGroupedCollection<string, Track> Results { get; }

    public async Task ExceuteSearchAsync()
    {
        var request = new SearchTrackRequest(QueryText, 2022);
        var results = await mediator.Send(request);

        var count = results.Count;
        ResultCount = count > 99
            ? string.Format(Translator.Instance["ResultsFound"], "100+")
            : string.Format(Translator.Instance["ResultsFound"], count);

        SortAndGroupResults(results);
    }

    public void OnActivate(ISortGroupNameProvider nameProvider)
    {
        if (GroupOptions.Count == 0)
        {
            GroupOptions.ClearAddRange(groupOptions.Select(x => new GroupViewModel(x, nameProvider.GetNameForGroup(x))));
            SortOptions.ClearAddRange(sortOptions.Select(x => new SortViewModel(x, nameProvider.GetNameForSort(x))));
            GroupBy = GroupOptions.First();
            SortBy = SortOptions.First();
        }
        else
        {
            foreach (var groupByOption in GroupOptions)
            {
                groupByOption.Name = nameProvider.GetNameForGroup(groupByOption.Value);
            }

            foreach (var sortByOption in SortOptions)
            {
                sortByOption.Name = nameProvider.GetNameForSort(sortByOption.Value);
            }
        }
    }

    public void ReSortGroup()
    {
        if (GroupBy is not null && SortBy is not null)
        {
            SortAndGroupResults(Results.SelectMany(x => x));
        }
    }

    private void SortAndGroupResults(IEnumerable<Track> results)
    {
        var sorted = SortBy.Value.Sort(results);
        var grouped = GroupBy.Value is GroupByNothing
            ? sorted.GroupBy(x => ResultCount)
            : GroupBy.Value.Group(sorted);

        var list = grouped.ToList();

        Results.ClearAddRange(list);
    }
}