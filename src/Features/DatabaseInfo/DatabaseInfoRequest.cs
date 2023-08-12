namespace Chroomsoft.Top2000.Features.DatabaseInfo;

public sealed class DatabaseInfoRequest : IRequest<int>
{
}

public sealed class DatabaseInfoRequestHandler : IRequestHandler<DatabaseInfoRequest, int>
{
    private readonly SQLiteAsyncConnection connection;

    public DatabaseInfoRequestHandler(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<int> Handle(DatabaseInfoRequest request, CancellationToken cancellationToken)
    {
        var sql = "SELECT ScriptName FROM Journal ORDER BY ScriptName DESC LIMIT 1";

        var idString = (await connection.QueryAsync<Journal>(sql))
            .Single()
            .ScriptName.Split('-')[0];

        return int.TryParse(idString, out var id)
            ? id
            : 0;
    }

    private sealed class Journal
    {
        public string ScriptName { get; set; } = string.Empty;
    }
}