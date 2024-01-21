using Chroomsoft.Top2000.Apps.Views.TrackInformation;
using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.Favorites;
using CommunityToolkit.Mvvm.Messaging;

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
    private readonly FavoritesHandler favoritesHandler;

    public OverviewByDateViewModel(IMediator mediator, FavoritesHandler favoritesHandler)
    {
        this.mediator = mediator;
        this.favoritesHandler = favoritesHandler;
        this.Listings = new();
        this.Dates = new();

        StrongReferenceMessenger.Default.Register<FavoriteChangedMessage>(this, (recipient, message) =>
        {
            if (recipient != message.Sender)
            {
                var item = Listings
                    .SelectMany(x => x)
                    .FirstOrDefault(x => x.Id == message.Value);

                if (item is not null)
                {
                    item.IsFavorite = message.IsFavorite;
                }
            }
        });
    }

    public ObservableGroupedCollection<DateTime, TrackListing> Listings { get; }

    public ObservableGroupedCollection<DateTime, DateTime> Dates { get; }

    public static DateTime LocalPlayDateAndTime(TrackListing listing) => listing.LocalPlayDateAndTime;

    [RelayCommand]
    public async Task ToggleFavoritesAsync(TrackListing listing)
    {
        listing.IsFavorite = !listing.IsFavorite;
        await favoritesHandler.SetIsFavorite(listing, listing.IsFavorite);

        StrongReferenceMessenger.Default.Send(new FavoriteChangedMessage(listing.Id)
        {
            Sender = this,
            IsFavorite = listing.IsFavorite,
        });
    }

    public async Task InitialiseViewModelAsync()
    {
        var editions = await mediator.Send(new AllEditionsRequest());
        SelectedEdition = editions.First();
        SelectedEditionYear = SelectedEdition.Year;

        await LoadAllListingsAsync();
    }

    [RelayCommand]
    public async Task NavigateToTrackListingAsync(TrackListing track)
    {
        await TrackInformationViewModel.NavigateAsync(track);
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

    private static DateTime LocalPlayDate(DateTime arg) => arg.Date;
}