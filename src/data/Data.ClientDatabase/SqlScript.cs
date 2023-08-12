namespace Chroomsoft.Top2000.Data.ClientDatabase;

public sealed class SqlScript
{
    public SqlScript(string scriptName, string contents)
    {
        ScriptName = scriptName;
        Contents = contents;
    }

    public string ScriptName { get; set; }

    public string Contents { get; }

    public string[] SqlSections()
    {
        const char OnSemicolon = ';';

        return Contents
            .Split(OnSemicolon)
            .Where(HasContent)
            .ToArray();
    }

    private static bool HasContent(string x) => !string.IsNullOrWhiteSpace(x);
}