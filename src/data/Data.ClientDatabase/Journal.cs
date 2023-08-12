namespace Chroomsoft.Top2000.Data.ClientDatabase;

public sealed class Journal
{
    public Journal()
    {
        ScriptName = string.Empty;
    }

    [PrimaryKey]
    public string ScriptName { get; set; }
}