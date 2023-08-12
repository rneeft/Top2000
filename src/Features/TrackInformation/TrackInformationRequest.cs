namespace Chroomsoft.Top2000.Features.TrackInformation;

public sealed class TrackInformationRequest : IRequest<TrackDetails>
{
    public TrackInformationRequest(int trackId)
    {
        TrackId = trackId;
    }

    public int TrackId { get; }
}

public sealed class TrackInformationRequestHandler : IRequestHandler<TrackInformationRequest, TrackDetails>
{
    private readonly SQLiteAsyncConnection connection;

    public TrackInformationRequestHandler(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<TrackDetails> Handle(TrackInformationRequest request, CancellationToken cancellationToken)
    {
        var sql =
            "SELECT Year AS Edition, Position, PlayUtcDateAndTime " +
            "FROM Edition LEFT JOIN Listing " +
            "ON Listing.Edition = Edition.Year AND Listing.TrackId = ?";

        var listings = (await connection.QueryAsync<ListingInformation>(sql, request.TrackId))
            .OrderBy(x => x.Edition)
            .ToArray();

        var track = await connection.GetAsync<Track>(request.TrackId);

        var statusStrategy = new ListingStatusStrategy(track.RecordedYear);

        ListingInformation? previous = null;

        foreach (var listing in listings)
        {
            if (previous != null && previous.Position.HasValue)
            {
                listing.Offset = listing.Position - previous.Position;
            }

            listing.Status = statusStrategy.Determine(listing);
            previous = listing;
        }

        return new TrackDetails
        {
            Title = track.Title,
            Artist = track.Artist,
            RecordedYear = track.RecordedYear,
            Listings = listings.ToImmutableSortedSet(new ListingInformationDescendingComparer()),
        };
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