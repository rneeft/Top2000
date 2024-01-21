namespace Chroomsoft.Top2000.Features.Favorites;

public sealed class FavoritesHandler
{
    private readonly SQLiteAsyncConnection connection;

    public FavoritesHandler(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public Task SetIsFavorite(BaseTrack track, bool isFavorite)
    {
        var sql = "UPDATE Track SET IsFavorite = ? WHERE Id = ?";

        return connection.ExecuteAsync(sql, isFavorite, track.Id);
    }

    public async Task<IReadOnlyList<FavoriteTrack>> GetAllAsync(int lastEdition)
    {
        var sql = "SELECT Id, Title, Artist, RecordedYear, ? AS LastEdition, Listing.Position AS Position " +
           "FROM Track " +
           "LEFT JOIN Listing ON Track.Id = Listing.TrackId AND Listing.Edition = ? " +
           "WHERE IsFavorite = true";

        var result = await connection.QueryAsync<FavoriteTrack>(sql, lastEdition, lastEdition);

        return result.AsReadOnly();
    }
}