namespace Chroomsoft.Top2000.Data.ClientDatabase.Sources;

public sealed class OnlineDataSource : ISource
{
    private readonly IHttpClientFactory httpClientFactory;

    public OnlineDataSource(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<ImmutableSortedSet<string>> ExecutableScriptsAsync(ImmutableSortedSet<string> journals)
    {
        if (journals.IsEmpty)
        {
            return ImmutableSortedSet<string>.Empty;
        }

        var latestVersion = journals
            .Last()
            .Split('-')[0];

        var content = await TryGetAsyncForUpgrades(latestVersion);

        if (content is null)
        {
            return ImmutableSortedSet<string>.Empty;
        }

        return content.ToImmutableSortedSet();
    }

    public async Task<SqlScript> ScriptContentsAsync(string scriptName)
    {
        var httpClient = httpClientFactory.CreateClient("top2000");

        var response = await httpClient.GetAsync(new Uri($"data/{scriptName}", UriKind.Relative));
        response.EnsureSuccessStatusCode();

        var contents = await response.Content.ReadAsStringAsync();

        return new SqlScript(scriptName, contents);
    }

    private async Task<IEnumerable<string>?> TryGetAsyncForUpgrades(string latestVersion)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("top2000");
            var requestUri = new Uri($"api/versions/{latestVersion}/upgrades", UriKind.Relative);
            var response = await httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<string>>() ?? null;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}