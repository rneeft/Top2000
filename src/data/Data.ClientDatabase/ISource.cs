using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.ClientDatabase
{
    public interface ISource
    {
        Task<ImmutableSortedSet<string>> ExecutableScriptsAsync(ImmutableSortedSet<string> journals);

        Task<SqlScript> ScriptContentsAsync(string scriptName);
    }

    //public class OnlineDataSource : ISource
    //{
    //    private readonly IHttpClientFactory httpClientFactory;

    //    public OnlineDataSource(IHttpClientFactory httpClientFactory)
    //    {
    //        this.httpClientFactory = httpClientFactory;
    //    }

    //    public Task<ImmutableSortedSet<SqlScript>> ExecutableScriptsAsync(IImmutableSet<string> journals)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}
