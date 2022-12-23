namespace Chroomsoft.Top2000.Features.AllListingsOfEdition;

public interface IAllListingsOfEdition
{
    Task<ImmutableHashSet<TrackListing>> ListingForEdition(int year);
}

public sealed class AllListingsOfEdition : IAllListingsOfEdition
{
    private readonly SQLiteAsyncConnection connection;

    public AllListingsOfEdition(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<ImmutableHashSet<TrackListing>> ListingForEdition(int year)
    {
        var sql =
          "SELECT Listing.TrackId, Listing.Position, (p.Position - Listing.Position) AS Delta, Listing.PlayUtcDateAndTime, Title, Artist " +
          "FROM Listing JOIN Track ON Listing.TrackId = Id " +
          "LEFT JOIN Listing as p ON p.TrackId = Id AND p.Edition = ? " +
          $"WHERE Listing.Edition = ?";

        var tracklisting = await connection.QueryAsync<TrackListing>(sql, year - 1, year);

        return tracklisting
            .ToImmutableHashSet(new TrackListingComparer());
    }
}