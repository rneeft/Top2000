using Chroomsoft.Top2000.Features.AllEditions;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByDate;

public sealed partial class OverviewByDateViewModel : ObservableObject
{
    [ObservableProperty]
    public TrackListing? selectedListing;

    [ObservableProperty]
    public Edition? selectedEdition;

    [ObservableProperty]
    public int selectedEditionYear;

    private readonly IMediator mediator;

    public OverviewByDateViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        this.Listings = new();
        this.Dates = new();
    }

    public ObservableGroupedCollection<DateTime, TrackListing> Listings { get; }

    public ObservableGroupedCollection<DateTime, DateTime> Dates { get; }

    public static DateTime LocalPlayDateAndTime(TrackListing listing) => listing.LocalPlayDateAndTime;

    public async Task InitialiseViewModelAsync()
    {
        var editions = await mediator.Send(new AllEditionsRequest());
        SelectedEdition = editions.First();
        SelectedEditionYear = SelectedEdition.Year;

        await LoadAllListingsAsync();
    }

    public async Task LoadAllListingsAsync()
    {
        var tracks = await mediator.Send(new AllListingsOfEditionRequest(SelectedEditionYear));
        var listings = tracks
            .OrderByDescending(x => x.Position)
            .GroupBy(LocalPlayDateAndTime);

        var dates = listings
            .Select(x => x.Key)
            .GroupBy(LocalPlayDate);

        Listings.ClearAddRange(listings);
        Dates.ClearAddRange(dates);
    }

    private DateTime LocalPlayDate(DateTime arg) => arg.Date;
}