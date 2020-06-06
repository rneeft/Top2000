using SQLite;

namespace Chroomsoft.Top2000.Data.ClientDatabase
{
    //public class Top2000AssemblyDataSource : ISource
    //{
    //    private readonly ITop2000AssemblyData top2000Data;

    //    public Top2000AssemblyDataSource(ITop2000AssemblyData top2000Data)
    //    {
    //        this.top2000Data = top2000Data;
    //    }

    //    private ImmutableSortedSet<string> ExecutableScripts(IImmutableSet<string> journals);

    //    private ImmutableSortedSet<string> GetScriptsToExecute(ISet<string> journals)
    //    {
    //        //return top2000Data
    //        //    .GetAllSqlFiles()
    //        //    .Except(journals);
    //    }
    //}

    //public class OnlineDataSource : ISource
    //{
    //    public OnlineDataSource(IHttpClientFactory httpClientFactory)
    //    {
    //    }
    //}

    //public class CreateAndUpgradeDatabase : IUpdateClientDatabase
    //{
    //    private readonly ITop2000AssemblyData top2000Data;
    //    private readonly ILogger<CreateAndUpgradeDatabase> logger;
    //    private readonly SQLiteAsyncConnection connection;

    //    public async Task GenerateOrUpdateAsync()
    //    {
    //        var journals = await GetJournalsAsync();
    //        var scripts = GetScriptsToExecute(journals);

    //                        .OrderBy(FileVersion)
    //            .ToImmutableSortedSet();

    //        foreach (var script in scripts)
    //        {
    //            var contents = await top2000Data.GetScriptContentAsync(script);
    //            await Transaction.RunAsync(connection, script, contents);
    //        }

    //        if (journals.Any())
    //        {
    //            var latestFileName = files.LastOrDefault() ?? journals.Last();
    //            var lastestVersion = FileVersion(latestFileName);

    //            await CheckOnlineUpgradesAsync(lastestVersion);
    //        }
    //    }

    //    private async Task CheckOnlineUpgradesAsync(int version)
    //    {
    //        var uri = new Uri($"api/versions/{version}/upgrades", UriKind.Relative);

    //        var httpClient = httpClientFactory.CreateClient("api");
    //        var response = await httpClient.GetAsync(uri);
    //        if (response.IsSuccessStatusCode)
    //        {
    //            using var responseStream = await response.Content.ReadAsStreamAsync();
    //            var upgrades = (await JsonSerializer.DeserializeAsync<IEnumerable<string>>(responseStream))
    //                .ToImmutableSortedSet();

    //            foreach (var upgrade in upgrades)
    //                await ExecuteOnlineScriptAsync(upgrade);
    //        }
    //    }

    //    private async Task ExecuteOnlineScriptAsync(string url)
    //    {
    //        var uri = new Uri(url, UriKind.Relative);

    //        var httpClient = httpClientFactory.CreateClient("api");
    //        var response = await httpClient.GetAsync(uri);

    //        if (response.IsSuccessStatusCode)
    //        {
    //            var contents = await response.Content.ReadAsStringAsync();
    //            var fileName = url.Replace("data/", string.Empty, StringComparison.OrdinalIgnoreCase);

    //            await ExecuteScriptUnderTransactionAsync(fileName, contents);
    //        }
    //    }

    //    private int FileVersion(string fileName)
    //    {
    //        return int.Parse(fileName.Split('-')[0].Trim(), CultureInfo.InvariantCulture.NumberFormat);
    //    }

    //    public class OnlineUpgrade
    //    {
    //        private readonly IHttpClientFactory httpClientFactory;

    //        public OnlineUpgrade(IHttpClientFactory httpClientFactory)
    //        {
    //            this.httpClientFactory = httpClientFactory;
    //        }

    //        public ImmutableSortedSet<string> Upgrades { get; set; }

    //        public async Task<bool> IsUpgradeAvailableAsync(int version)
    //        {
    //            var response = await GetUpgradesResponseAsync(version);

    //            if (response.IsSuccessStatusCode)
    //                Upgrades = await TransformResponseAsync(response);

    //            return Upgrades.Any();

    //            var uri = new Uri($"api/versions/{version}/upgrades", UriKind.Relative);

    //            var httpClient = httpClientFactory.CreateClient("api");
    //            var response = await httpClient.GetAsync(uri);
    //            if (response.IsSuccessStatusCode)
    //            {
    //                using var responseStream = await response.Content.ReadAsStreamAsync();
    //                var upgrades = (await JsonSerializer.DeserializeAsync<IEnumerable<string>>(responseStream))
    //                    .ToImmutableSortedSet();

    //                foreach (var upgrade in upgrades)
    //                    await ExecuteOnlineScriptAsync(upgrade);
    //            }
    //        }

    //        private async Task<ImmutableSortedSet<string>> TransformResponseAsync(HttpResponseMessage response)
    //        {
    //            using var responseStream = await response.Content.ReadAsStreamAsync();
    //            return (await JsonSerializer.DeserializeAsync<IEnumerable<string>>(responseStream))
    //                .ToImmutableSortedSet();
    //        }

    //        private Task<HttpResponseMessage> GetUpgradesResponseAsync(int version)
    //        {
    //            var uri = new Uri($"api/versions/{version}/upgrades", UriKind.Relative);

    //            var httpClient = httpClientFactory.CreateClient("api");
    //            return httpClient.GetAsync(uri);
    //        }
    //    }
    //}

    public class Journal
    {
        public Journal()
        {
            ScriptName = string.Empty;
        }

        [PrimaryKey]
        public string ScriptName { get; set; }
    }
}
