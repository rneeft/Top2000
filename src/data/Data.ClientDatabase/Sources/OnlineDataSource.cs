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
            return ImmutableSortedSet<string>.Empty;

        const char OnDash = '-';

        var latestVersion = journals
            .Last()
            .Split(OnDash)[0];

        var content = await TryGetAsyncForUpgrades(latestVersion);

        return content is null
            ? ImmutableSortedSet<string>.Empty
            : content.ToImmutableSortedSet();
    }

    public async Task<SqlScript> ScriptContentsAsync(string scriptName)
    {
        var httpClient = httpClientFactory.CreateClient("top2000");
        var requestUri = $"data/{scriptName}";

        var response = await httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var contents = await response.Content.ReadAsStringAsync();

        return new SqlScript(scriptName, contents);
    }

    private async Task<IEnumerable<string>?> TryGetAsyncForUpgrades(string latestVersion)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("top2000");
            var requestUri = $"api/versions/{latestVersion}/upgrades";
            var response = await httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            return await JsonSerializer.DeserializeAsync<IEnumerable<string>>(response.Content.ReadAsStream());
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}