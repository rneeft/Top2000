namespace Chroomsoft.Top2000.Data.ClientDatabase.Sources;

public interface ISource
{
    Task<ImmutableSortedSet<string>> ExecutableScriptsAsync(ImmutableSortedSet<string> journals);

    Task<SqlScript> ScriptContentsAsync(string scriptName);
}