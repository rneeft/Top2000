namespace Chroomsoft.Top2000.Data.ClientDatabase;

public interface IDatbaseInfo
{
    Task<Journal?> LastDatabaseVersionAsync();
}

public sealed class DatabaseInfo : IDatbaseInfo
{
    private readonly SQLiteAsyncConnection connection;

    public DatabaseInfo(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task<Journal?> LastDatabaseVersionAsync()
    {
        var sql = "SELECT ScriptName FROM Journal ORDER BY ScriptName DESC LIMIT 1";
        var journals = await connection.QueryAsync<Journal>(sql);

        return journals.FirstOrDefault();
    }
}