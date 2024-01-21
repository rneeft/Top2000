namespace Chroomsoft.Top2000.Features.AllListingsOfEdition;

public sealed class AllListingsOfEditionRequest : IRequest<ImmutableHashSet<TrackListing>>
{
    public AllListingsOfEditionRequest(int year)
    {
        Year = year;
    }

    public int Year { get; }
}

public sealed class AllListingsOfEditionRequestHandler : IRequestHandler<AllListingsOfEditionRequest, ImmutableHashSet<TrackListing>>
{
    private readonly SQLiteAsyncConnection connection;

    public AllListingsOfEditionRequestHandler(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<ImmutableHashSet<TrackListing>> Handle(AllListingsOfEditionRequest request, CancellationToken cancellationToken)
    {
        var sql =
                  "SELECT Id, Listing.Position, (p.Position - Listing.Position) AS Delta, Listing.PlayUtcDateAndTime, Title, Artist, IsFavorite " +
                  "FROM Listing JOIN Track ON Listing.TrackId = Id " +
                  "LEFT JOIN Listing as p ON p.TrackId = Id AND p.Edition = ? " +
                  $"WHERE Listing.Edition = ?";

        var tracks = await connection.QueryAsync<TrackListing>(sql, request.Year - 1, request.Year);
        return tracks.ToImmutableHashSet(new TrackListingComparer());
    }
}