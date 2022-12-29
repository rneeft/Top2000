namespace Chroomsoft.Top2000.Data.ClientDatabase.Model;

public sealed class Journal
{
    public Journal()
    {
        ScriptName = string.Empty;
    }

    [PrimaryKey]
    public string ScriptName { get; set; }

    public int Version => int.Parse(ScriptName.Split('-')[0]);
}