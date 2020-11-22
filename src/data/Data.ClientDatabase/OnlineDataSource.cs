using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.ClientDatabase
{
    public class OnlineDataSource : ISource
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

            var latestVersion = journals
                .Last()
                .Split('-')[0];

            var content = await TryGetAsyncForUpgrades(latestVersion).ConfigureAwait(false);

            if (content is null)
                return ImmutableSortedSet<string>.Empty;

            return JsonConvert.DeserializeObject<IEnumerable<string>>(content)
                .ToImmutableSortedSet();
        }

        public async Task<SqlScript> ScriptContentsAsync(string scriptName)
        {
            var httpClient = httpClientFactory.CreateClient("top2000");
            var requestUri = $"data/{scriptName}";

            var response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var contents = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new SqlScript(scriptName, contents);
        }

        private async Task<string?> TryGetAsyncForUpgrades(string latestVersion)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("top2000");
                var requestUri = $"api/versions/{latestVersion}/upgrades";
                var response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}
