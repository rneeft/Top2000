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

    public class Top2000AssemblyDataSource : ISource
    {
        private readonly ITop2000AssemblyData top2000Data;

        public Top2000AssemblyDataSource(ITop2000AssemblyData top2000Data)
        {
            this.top2000Data = top2000Data;
        }

        public Task<ISet<string>> ExecutableScriptsAsync() => Task.FromResult(top2000Data.GetAllSqlFiles());

        public async Task<SqlScript> ScriptContentsAsync(string scriptName)
        {
            var contents = await top2000Data.GetScriptContentAsync(scriptName);
            return new SqlScript(scriptName, contents);
        }
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
