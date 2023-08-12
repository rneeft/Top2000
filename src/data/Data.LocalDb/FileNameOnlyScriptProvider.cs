namespace Chroomsoft.Top2000.Data.LocalDb;

public sealed class Top2000DataScriptProvider : IScriptProvider
{
    private readonly ITop2000AssemblyData top2000AssemblyData;

    public Top2000DataScriptProvider(ITop2000AssemblyData top2000AssemblyData)
    {
        this.top2000AssemblyData = top2000AssemblyData;
    }

    public IEnumerable<SqlScript> GetScripts(IConnectionManager connectionManager)
    {
        var filesAsTasks = top2000AssemblyData
            .GetAllSqlFiles()
            .Select(BuildSqlScriptAsync);

        return Task.WhenAll(filesAsTasks).GetAwaiter().GetResult();
    }

    private async Task<SqlScript> BuildSqlScriptAsync(string fileName)
    {
        var contents = await top2000AssemblyData.GetScriptContentAsync(fileName).ConfigureAwait(false);
        return new SqlScript(fileName, contents);
    }
}