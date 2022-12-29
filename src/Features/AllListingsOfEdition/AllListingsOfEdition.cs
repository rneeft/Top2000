namespace Chroomsoft.Top2000.Features;

public interface IAllListingsOfEdition
{
    Task<ImmutableHashSet<TrackListing>> ListingForEdition(Edition edition);
}

public sealed class AllListingsOfEdition : IAllListingsOfEdition
{
    private readonly SQLiteAsyncConnection connection;

    public AllListingsOfEdition(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<ImmutableHashSet<TrackListing>> ListingForEdition(Edition edition)
    {
        var sql =
          "SELECT Listing.TrackId, Listing.Position, (p.Position - Listing.Position) AS Delta, Listing.PlayUtcDateAndTime, Title, Artist " +
          "FROM Listing JOIN Track ON Listing.TrackId = Id " +
          "LEFT JOIN Listing as p ON p.TrackId = Id AND p.Edition = ? " +
          $"WHERE Listing.Edition = ?";

        var previousEdition = edition.Year - 1;
        var tracklisting = await connection.QueryAsync<TrackListing>(sql, previousEdition, edition.Year);

        return tracklisting
            .ToImmutableHashSet(new TrackListingComparer());
    }
}