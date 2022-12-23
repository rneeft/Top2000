namespace Chroomsoft.Top2000.Features.AllEditions;

public sealed class AllEditions
{
    private readonly SQLiteAsyncConnection connection;

    public AllEditions(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<ImmutableSortedSet<Edition>> Editions()
    {
        var allEditions = await connection.Table<Edition>().ToListAsync();

        return allEditions.ToImmutableSortedSet(new EditionDescendingComparer());
    }
}