using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using MediatR;

namespace Chroomsoft.Top2000.Apps.Overview.ByPosition;

public sealed partial class OverviewByPositionViewModel : ObservableObject
{
    [ObservableProperty]
    public TrackListing? selectedListing;

    [ObservableProperty]
    public Edition? selectedEdition;

    [ObservableProperty]
    public int selectedEditionYear;

    private readonly IMediator mediator;

    public OverviewByPositionViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        this.Editions = new();
        this.Listings = new();
    }

    public ObservableRangeCollection<Edition> Editions { get; }
    public ObservableGroupedCollection<string, TrackListing> Listings { get; }

    public static string Position(TrackListing listing)
    {
        const int GroupSize = 100;

        if (listing.Position < 100) return "1 - 100";
        if (listing.Position >= 1900) return "1900 - 2000";

        var min = listing.Position / GroupSize * GroupSize;
        var max = min + GroupSize;

        return $"{min} - {max}";
    }

    [RelayCommand]
    public async Task InitialiseViewModelAsync()
    {
        var editions = await mediator.Send(new AllEditionsRequest());
        SelectedEdition = editions.First();
        SelectedEditionYear = SelectedEdition.Year;
        Editions.ClearAddRange(editions);

        await LoadAllListingsAsync();
    }

    public async Task LoadAllListingsAsync()
    {
        if (SelectedEdition is null) return;

        var listings = await mediator.Send(new AllListingsOfEditionRequest(SelectedEdition.Year));

        Listings.ClearAddRange(listings.GroupBy(Position));

        SelectedListing = null;
    }
}