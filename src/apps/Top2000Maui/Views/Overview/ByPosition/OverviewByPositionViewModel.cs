using Chroomsoft.Top2000.Apps.Views.TrackInformation;
using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.Favorites;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;

public sealed partial class OverviewByPositionViewModel : ObservableObject
{
    [ObservableProperty]
    public TrackListing? selectedListing;

    [ObservableProperty]
    public int selectedEditionYear;

    private readonly IMediator mediator;
    private readonly FavoritesHandler favoritesHandler;

    public OverviewByPositionViewModel(IMediator mediator, FavoritesHandler favoritesHandler)
    {
        this.mediator = mediator;
        this.favoritesHandler = favoritesHandler;
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
    public async Task ToggleFavoritesAsync(TrackListing listing)
    {
        listing.IsFavorite = !listing.IsFavorite;
        await favoritesHandler.SetIsFavorite(listing, (bool)listing.IsFavorite);
    }

    [RelayCommand]
    public async Task NavigateToTrackListingAsync(TrackListing track)
    {
        await TrackInformationViewModel.NavigateAsync(track);
    }

    [RelayCommand]
    public async Task InitialiseViewModelAsync()
    {
        var editions = await mediator.Send(new AllEditionsRequest());
        SelectedEditionYear = editions.First().Year;

        await LoadAllListingsAsync();
    }

    public async Task InitialiseViewModelAsync(int year)
    {
        SelectedEditionYear = year;

        await LoadAllListingsAsync();
    }

    public async Task LoadAllListingsAsync()
    {
        if (SelectedEditionYear == 0) return;

        var listings = await mediator.Send(new AllListingsOfEditionRequest(SelectedEditionYear));

        Listings.ClearAddRange(listings.GroupBy(Position));

        SelectedListing = null;
    }
}