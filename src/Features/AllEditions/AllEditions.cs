namespace Chroomsoft.Top2000.Features.Editions;

public interface IAllEditions
{
    Task<ImmutableSortedSet<Edition>> Editions();
}

public sealed class AllEditions : IAllEditions
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