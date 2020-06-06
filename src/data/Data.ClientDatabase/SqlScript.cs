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

        public static bool operator ==(SqlScript left, SqlScript right) => left.Equals(right);

        public static bool operator !=(SqlScript left, SqlScript right) => !(left == right);

        public static bool operator <(SqlScript left, SqlScript right) => left.CompareTo(right) < 0;

        public static bool operator <=(SqlScript left, SqlScript right) => left.CompareTo(right) <= 0;

        public static bool operator >(SqlScript left, SqlScript right) => left.CompareTo(right) > 0;

        public static bool operator >=(SqlScript left, SqlScript right) => left.CompareTo(right) >= 0;

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

        public override bool Equals(object obj)
        {
            if (obj is SqlScript script)
            {
                return script.ScriptName == this.ScriptName &&
                    script.Contents == this.Contents;
            }

            return false;
        }

        public override int GetHashCode() => ScriptName.GetHashCode();

        private static bool HasContent(string x) => !string.IsNullOrWhiteSpace(x);
    }
}
