namespace Chroomsoft.Top2000.Features.AllEditions;

public sealed class AllEditionsRequest : IRequest<ImmutableSortedSet<Edition>>
{
}

public sealed class AllEditionsRequestHandler : IRequestHandler<AllEditionsRequest, ImmutableSortedSet<Edition>>
{
    private readonly SQLiteAsyncConnection connection;

    public AllEditionsRequestHandler(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<ImmutableSortedSet<Edition>> Handle(AllEditionsRequest request, CancellationToken cancellationToken)
    {
        var editions = await connection.Table<Edition>().ToListAsync();

        return editions.ToImmutableSortedSet(new EditionDescendingComparer());
    }
}