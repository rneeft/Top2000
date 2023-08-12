using System.Collections.ObjectModel;

namespace Chroomsoft.Top2000.Features.Searching;

public sealed class SearchTrackRequest : IRequest<ReadOnlyCollection<IGrouping<string, Track>>>
{
    public required string QueryString { get; init; }

    public required ISort Sorting { get; set; }

    public required IGroup Grouping { get; set; }

    public required int LastEdition { get; init; }
}

public sealed class SearchTrackRequestHandler : IRequestHandler<SearchTrackRequest, ReadOnlyCollection<IGrouping<string, Track>>>
{
    private readonly SQLiteAsyncConnection connection;

    public SearchTrackRequestHandler(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<ReadOnlyCollection<IGrouping<string, Track>>> Handle(SearchTrackRequest request, CancellationToken cancellationToken)
    {
        var results = await SearchDatabaseAsync(request.QueryString, request.LastEdition);

        var sorted = request.Sorting.Sort(results);
        var groupedAndSorted = request.Grouping.Group(sorted).ToList();

        return groupedAndSorted.AsReadOnly();
    }

    private Task<List<Track>> SearchDatabaseAsync(string queryString, int lastEdition)
    {
        if (string.IsNullOrWhiteSpace(queryString))
        {
            return Task.FromResult(new List<Track>());
        }

        return int.TryParse(queryString, out var edition)
            ? QuearyAsEditionAsync(edition, lastEdition)
            : QueryOnTitleAndArtist(queryString, lastEdition);
    }

    private Task<List<Track>> QuearyAsEditionAsync(int recordedYear, int lastEdition)
    {
        var sql = "SELECT Id, Title, Artist, RecordedYear, Listing.Position AS Position " +
                  "FROM Track " +
                  "LEFT JOIN Listing ON Track.Id = Listing.TrackId AND Listing.Edition = ? " +
                  "WHERE RecordedYear = ?" +
                  "LIMIT 100";

        return connection.QueryAsync<Track>(sql, lastEdition, recordedYear);
    }

    private Task<List<Track>> QueryOnTitleAndArtist(string queryString, int lastEdition)
    {
        var sql = "SELECT Id, Title, Artist, RecordedYear, Listing.Position AS Position " +
                   "FROM Track " +
                   "LEFT JOIN Listing ON Track.Id = Listing.TrackId AND Listing.Edition = ? " +
                   "WHERE (Title LIKE ?) OR (Artist LIKE ?)" +
                   "LIMIT 100";

        return connection.QueryAsync<Track>(sql, lastEdition, $"%{queryString}%", $"%{queryString}%");
    }
}