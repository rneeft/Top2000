using System;
using System.Linq;

namespace Chroomsoft.Top2000.Data.ClientDatabase
{
    public class SqlScript : IComparable<SqlScript>
    {
        private const char OnSemicolon = ';';

        public SqlScript(string scriptName, string contents)
        {
            ScriptName = scriptName;
            Contents = contents;
        }

        public string ScriptName { get; set; }

        public string Contents { get; }

        public string[] SqlSections()
        {
            return Contents
                .Split(OnSemicolon)
                .Where(HasContent)
                .ToArray();
        }

        public int CompareTo(SqlScript other)
        {
            return string.Compare(ScriptName, other.ScriptName, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool HasContent(string x) => !string.IsNullOrWhiteSpace(x);
    }
}
