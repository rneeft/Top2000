using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator
{
    public interface ITransformSqlFiles
    {
        IReadOnlyCollection<VersionFile> Transform();
    }

    public class SqlFileTransformer : ITransformSqlFiles
    {
        private readonly ITop2000AssemblyData top2000Data;

        public SqlFileTransformer(ITop2000AssemblyData top2000Data)
        {
            this.top2000Data = top2000Data;
        }

        public IReadOnlyCollection<VersionFile> Transform()
        {
            var allVersions = top2000Data
                .GetAllSqlFiles()
                .Select(TransformToVersionFile)
                .ToList();

            var allVersionsCopy = allVersions.ToList();

            foreach (var version in allVersions)
            {
                allVersionsCopy.Remove(version);

                version.AddRange(allVersionsCopy);
            }

            return allVersions;
        }

        private VersionFile TransformToVersionFile(string fileName)
        {
            var split = fileName.Split('-');
            var index = int.Parse(split[0], CultureInfo.InvariantCulture.NumberFormat);

            return new VersionFile(index, fileName);
        }
    }
}
