namespace Chroomsoft.Top2000.Apps.Views.TrackInformation;

public sealed partial class TrackInformationViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    public string artistWithYear = string.Empty;

    private readonly IMediator mediator;
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
    private double appearancesPossiblePercentage;

    [ObservableProperty]
    private double totalTop2000Percentage;

    public TrackInformationViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        this.Listings = new();
    }

    public ObservableRangeCollection<ListingInformation> Listings { get; }

    public static Task NavigateAsync(int trackId, string title, string artist)
    {
        var parameters = new Dictionary<string, object>
        {
            { "Title", title  },
            { "Artist", artist },
            { "TrackId", trackId },
        };

        return Shell.Current.GoToAsync(nameof(TrackInformationPage), animate: true, parameters);
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
        AppearancesPossiblePercentage = (Appearances / (double)AppearancesPossible);
        TotalTop2000Percentage = (Appearances / (double)Listings.Count);
        TotalListings = Listings.Count;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        trackId = (int)query["TrackId"];
        Title = (string)query["Title"];
        Artist = (string)query["Artist"];
        ArtistWithYear = (string)query["Artist"];
    }
}