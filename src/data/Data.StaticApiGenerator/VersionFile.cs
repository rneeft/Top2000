using System.Collections.Generic;
using System.Diagnostics;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator
{
    [DebuggerDisplay("{FileName}")]
    public class VersionFile
    {
        private List<VersionFile> upgrades;

        public VersionFile(int version, string fileName)
        {
            this.Version = version;
            this.FileName = fileName;
            this.upgrades = new List<VersionFile>();
        }

        public int Version { get; set; }

        public string FileName { get; set; }

        public IReadOnlyCollection<VersionFile> Upgrades => upgrades;

        public void AddRange(IEnumerable<VersionFile> versionFiles)
        {
            upgrades.AddRange(versionFiles);
        }
    }
}
