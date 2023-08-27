namespace Chroomsoft.Top2000.Features.Searching;

public record SearchTrackRequest(string QueryString, int LastEdition) : IRequest<ReadOnlyCollection<Track>>;

public sealed class SearchTrackRequestHandler : IRequestHandler<SearchTrackRequest, ReadOnlyCollection<Track>>
{
    private readonly SQLiteAsyncConnection connection;

    public SearchTrackRequestHandler(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<ReadOnlyCollection<Track>> Handle(SearchTrackRequest request, CancellationToken cancellationToken)
    {
        var results = await SearchDatabaseAsync(request.QueryString, request.LastEdition);
        return results.AsReadOnly();
    }

    private Task<List<Track>> SearchDatabaseAsync(string queryString, int lastEdition)
    {
        return int.TryParse(queryString, out var edition)
            ? QuearyAsEditionAsync(edition, lastEdition)
            : QueryOnTitleAndArtist(queryString, lastEdition);
    }

    private Task<List<Track>> QuearyAsEditionAsync(int recordedYear, int lastEdition)
    {
        var sql = "SELECT Id, Title, Artist, RecordedYear, ? AS LastEdition, Listing.Position AS Position " +
                  "FROM Track " +
                  "LEFT JOIN Listing ON Track.Id = Listing.TrackId AND Listing.Edition = ? " +
                  "WHERE RecordedYear = ?" +
                  "LIMIT 100";

        return connection.QueryAsync<Track>(sql, lastEdition, lastEdition, recordedYear);
    }

    private Task<List<Track>> QueryOnTitleAndArtist(string queryString, int lastEdition)
    {
        var sql = "SELECT Id, Title, Artist, RecordedYear, ? AS LastEdition, Listing.Position AS Position " +
                   "FROM Track " +
                   "LEFT JOIN Listing ON Track.Id = Listing.TrackId AND Listing.Edition = ? " +
                   "WHERE (Title LIKE ?) OR (Artist LIKE ?)" +
                   "LIMIT 100";

        return connection.QueryAsync<Track>(sql, lastEdition, lastEdition, $"%{queryString}%", $"%{queryString}%");
    }
}