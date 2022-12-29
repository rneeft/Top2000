namespace Chroomsoft.Top2000.Features.TrackInformation;

public interface ICalculateTrackDetails
{
    Task<TrackDetails> Calculate(int trackId);
}

public sealed class TrackDetailsCalculator : ICalculateTrackDetails
{
    private readonly SQLiteAsyncConnection connection;

    public TrackDetailsCalculator(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<TrackDetails> Calculate(int trackId)
    {
        var track = await connection.GetAsync<Track>(trackId);
        var statusStrategy = new ListingStatusStrategy(track.RecordedYear);
        var listings = await ListingsAsync(statusStrategy, trackId).ToArrayAsync();

        var listingsWithPosition = listings
            .Where(x => x.Position.HasValue)
            .OrderBy(x => x.Position)
            .ThenBy(x => x.Edition);

        return new TrackDetails
        {
            Artist = track.Artist,
            Title = track.Title,
            RecordedYear = track.RecordedYear,
            Listings = listings.ToImmutableSortedSet(new ListingInformationDescendingComparer()),
            First = listings.First(x => x.Status == ListingStatus.New),
            Latest = listings.First(x => x.Position.HasValue),
            Appearances = listings.Count(x => x.Position.HasValue),
            AppearancesPossible = listings.Count(x => x.Status != ListingStatus.NotAvailable),
            Highest = listingsWithPosition.First(),
            Lowest = listingsWithPosition.Last()
        };
    }

    private async IAsyncEnumerable<Listing> ListingsAsync(ListingStatusStrategy statusStrategy, int trackId)
    {
        var sql =
           "SELECT Year AS Edition, Position, PlayUtcDateAndTime " +
           "FROM Edition LEFT JOIN Listing " +
           "ON Listing.Edition = Edition.Year AND Listing.TrackId = ?";

        var listings = await connection.QueryAsync<Listing>(sql, trackId);
        listings = listings.OrderBy(x => x.Edition).ToList();

        Listing? previous = null;

        foreach (var listing in listings)
        {
            if (previous != null && previous.Position.HasValue)
            {
                listing.Offset = listing.Position - previous.Position;
            }

            listing.Status = statusStrategy.Determine(listing);
            previous = listing;

            yield return listing;
        }
    }

    private sealed class Track
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;

        public int RecordedYear { get; set; } = 1;
    }
}