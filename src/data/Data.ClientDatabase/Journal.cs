using SQLite;

namespace Chroomsoft.Top2000.Data.ClientDatabase
{
    public class Journal
    {
        public Journal()
        {
            ScriptName = string.Empty;
        }

        [PrimaryKey]
        public string ScriptName { get; set; }
    }
}
