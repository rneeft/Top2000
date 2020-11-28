using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator
{
    [DebuggerDisplay("{FileName}")]
    public class VersionFile
    {
        private readonly List<VersionFile> upgrades;

        public VersionFile(string fileName)
        {
            this.Version = fileName.Split('-').First();
            this.FileName = fileName;
            this.upgrades = new List<VersionFile>();
        }

        public string Version { get; set; }

        public string FileName { get; set; }

        public IReadOnlyCollection<VersionFile> Upgrades => upgrades;

        public void AddRange(IEnumerable<VersionFile> versionFiles)
        {
            upgrades.AddRange(versionFiles);
        }
    }
}
