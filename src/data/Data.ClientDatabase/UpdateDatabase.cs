namespace Chroomsoft.Top2000.Data.ClientDatabase;

public interface IUpdateClientDatabase
{
    Task RunAsync(ISource source);
}

public sealed class UpdateDatabase : IUpdateClientDatabase
{
    private readonly SQLiteAsyncConnection connection;

    public UpdateDatabase(SQLiteAsyncConnection connection)
    {
        this.connection = connection;
    }

    public async Task RunAsync(ISource source)
    {
        var journals = await AllJournalsAsync();
        var executableScripts = await source.ExecutableScriptsAsync(journals);

        foreach (var scriptName in executableScripts)
        {
            var script = await source.ScriptContentsAsync(scriptName);
            await ExecuteScriptAsync(script);
        }
    }

    private Task ExecuteScriptAsync(SqlScript script)
    {
        return connection.RunInTransactionAsync(connection =>
        {
            var sections = script.SqlSections();

            foreach (var section in sections)
            {
                connection.Execute(section);
            }

            connection.Insert(new Journal { ScriptName = script.ScriptName });
        });
    }

    private async Task<ImmutableSortedSet<string>> AllJournalsAsync()
    {
        await connection.CreateTableAsync<Journal>();

        var journals = await connection.Table<Journal>().ToListAsync();

        return journals
            .Select(x => x.ScriptName)
            .ToImmutableSortedSet();
    }
}