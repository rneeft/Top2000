using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.ClientDatabase
{
    public interface ISource
    {
        Task<ImmutableSortedSet<string>> ExecutableScriptsAsync(ImmutableSortedSet<string> journals);

        Task<SqlScript> ScriptContentsAsync(string scriptName);
    }
}
