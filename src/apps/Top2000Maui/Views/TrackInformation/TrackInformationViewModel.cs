using Chroomsoft.Top2000.Features;
using Chroomsoft.Top2000.Features.Favorites;

namespace Chroomsoft.Top2000.Apps.Views.TrackInformation;

public sealed partial class TrackInformationViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    public string artistWithYear = string.Empty;

    private readonly IMediator mediator;
    private readonly FavoritesHandler favoritesHandler;
    private BaseTrack? baseTrack;
    private int trackId;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string artist = string.Empty;

    [ObservableProperty]
    private int totalListings;

    [ObservableProperty]
    private ListingInformation? highest;

    [ObservableProperty]
    private ListingInformation? lowest;

    [ObservableProperty]
    private ListingInformation? latest;

    [ObservableProperty]
    private ListingInformation? first;

    [ObservableProperty]
    private int appearances;

    [ObservableProperty]
    private bool isLatestListed;

    [ObservableProperty]
    private int appearancesPossible;

    [ObservableProperty]
    private int appearancesPossiblePercentage;

    [ObservableProperty]
    private int totalTop2000Percentage;

    [ObservableProperty]
    private bool isFavorite;

    public TrackInformationViewModel(IMediator mediator, FavoritesHandler favoritesHandler)
    {
        this.mediator = mediator;
        this.favoritesHandler = favoritesHandler;
        this.Listings = new();
    }

    public ObservableRangeCollection<ListingInformation> Listings { get; }

    public static Task NavigateAsync(BaseTrack baseTrack)
    {
        var parameters = new Dictionary<string, object>
        {
            { "Track", baseTrack  },
        };

        return Shell.Current.GoToAsync(nameof(TrackInformationPage), animate: true, parameters);
    }

    [RelayCommand]
    public async Task ToggleFavoritesAsync()
    {
        IsFavorite = !IsFavorite;
        baseTrack!.IsFavorite = IsFavorite;
        await favoritesHandler.SetIsFavorite(baseTrack, (bool)baseTrack.IsFavorite);
    }

    [RelayCommand]
    public async Task LoadTrackDetailsAsync()
    {
        var track = await mediator.Send(new TrackInformationRequest(trackId));

        Title = track.Title;
        ArtistWithYear = $"{track.Artist} ({track.RecordedYear})";
        Artist = track.Artist;
        Highest = track.Highest;
        Lowest = track.Lowest;
        Latest = track.Latest;
        First = track.First;
        Appearances = track.Appearances;
        AppearancesPossible = track.AppearancesPossible;
        IsLatestListed = track.Listings.First().Status != ListingStatus.NotListed;
        Listings.ClearAddRange(track.Listings);
        AppearancesPossiblePercentage = (int)(Appearances / (double)AppearancesPossible) * 100;
        TotalTop2000Percentage = (int)(Appearances / (double)Listings.Count) * 100;
        TotalListings = Listings.Count;
        IsFavorite = baseTrack!.IsFavorite;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        baseTrack = (BaseTrack)query["Track"];

        trackId = baseTrack.Id;
        Title = baseTrack.Title;
        Artist = baseTrack.Artist;
        ArtistWithYear = baseTrack.Artist;
    }
}