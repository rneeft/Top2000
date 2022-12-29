namespace Chroomsoft.Top2000.Features.Searching;

public interface ISearch
{
    Task<ReadOnlyCollection<IGrouping<string, Track>>> SearchByRecordedYearAsync(int year, int lastEdition, ISortSearch sorting, IGroupSearch grouping);
    Task<ReadOnlyCollection<IGrouping<string, Track>>> SearchByArtistTitleAsync(string query, int lastEdition, ISortSearch sorting, IGroupSearch grouping);
}

public sealed class Search : ISearch
{
    private readonly SQLiteAsyncConnection connection;

    public Search(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<ReadOnlyCollection<IGrouping<string, Track>>> SearchByRecordedYearAsync(int year, int lastEdition, ISortSearch sorting, IGroupSearch grouping)
    {
        var sql = "SELECT Id, Title, Artist, RecordedYear, Listing.Position AS Position " +
                  "FROM Track " +
                  "LEFT JOIN Listing ON Track.Id = Listing.TrackId AND Listing.Edition = ? " +
                  "WHERE RecordedYear = ?" +
                  "LIMIT 100";

        var results = await connection.QueryAsync<Track>(sql, lastEdition, year);

        var sorted = sorting.Sort(results);
        var groupedAndSorted = grouping.Group(sorted).ToList();

        return groupedAndSorted.AsReadOnly();
    }

    public async Task<ReadOnlyCollection<IGrouping<string, Track>>> SearchByArtistTitleAsync(string query, int lastEdition, ISortSearch sorting, IGroupSearch grouping)
    {
        var sql = "SELECT Id, Title, Artist, RecordedYear, Listing.Position AS Position " +
                   "FROM Track " +
                   "LEFT JOIN Listing ON Track.Id = Listing.TrackId AND Listing.Edition = ? " +
                   "WHERE (Title LIKE ?) OR (Artist LIKE ?)" +
                   "LIMIT 100";

        var results = await connection.QueryAsync<Track>(sql, lastEdition, $"%{query}%", $"%{query}%");

        var sorted = sorting.Sort(results);
        var groupedAndSorted = grouping.Group(sorted).ToList();

        return groupedAndSorted.AsReadOnly();
    }
}