using SQLite;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.ClientDatabase
{
    public interface IUpdateClientDatabase
    {
        Task RunAsync(ISource source);
    }

    public class UpdateDatabase : IUpdateClientDatabase
    {
        private readonly SQLiteAsyncConnection connection;

        public UpdateDatabase(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task RunAsync(ISource source)
        {
            var journals = await AllJournalsAsync().ConfigureAwait(false);
            var executableScripts = await source.ExecutableScriptsAsync(journals).ConfigureAwait(false);

            foreach (var scriptName in executableScripts)
            {
                var script = await source.ScriptContentsAsync(scriptName).ConfigureAwait(false);
                await ExecuteScriptAsync(script).ConfigureAwait(false);
            }
        }

        private Task ExecuteScriptAsync(SqlScript script)
        {
            return connection.RunInTransactionAsync(connection =>
            {
                var sections = script.SqlSections();

                foreach (var section in sections)
                    connection.Execute(section);

                connection.Insert(new Journal { ScriptName = script.ScriptName });
            });
        }

        private async Task<ImmutableSortedSet<string>> AllJournalsAsync()
        {
            await connection.CreateTableAsync<Journal>().ConfigureAwait(false);
            return (await connection.Table<Journal>().ToListAsync().ConfigureAwait(false))
                .Select(x => x.ScriptName)
                .ToImmutableSortedSet();
        }
    }
}
