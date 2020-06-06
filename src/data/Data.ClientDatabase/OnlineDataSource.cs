using System.Collections.Generic;
using System.Threading.Tasks;

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

    public interface ISource
    {
        Task<ISet<string>> ExecutableScriptsAsync();

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
