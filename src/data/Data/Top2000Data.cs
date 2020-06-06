using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data
{
    public interface ITop2000AssemblyData
    {
        Assembly DataAssembly { get; }

        Task<string> GetScriptContentAsync(string fileName);

        Stream GetScriptStream(string fileName);

        IReadOnlyCollection<string> GetAllSqlFiles();
    }

    public class Top2000Data : ITop2000AssemblyData
    {
        private readonly Func<string, bool> IsSqlFile = x => x.EndsWith(".sql", StringComparison.OrdinalIgnoreCase);
        private readonly string prefix;

        public Top2000Data()
        {
            prefix = DataAssembly.GetName().Name + ".sql.";
        }

        public Assembly DataAssembly => typeof(Top2000Data).Assembly;

        public Task<string> GetScriptContentAsync(string fileName)
        {
            using var stream = DataAssembly.GetManifestResourceStream(prefix + fileName) ?? throw new FileNotFoundException($"Unable to find {fileName} in {DataAssembly.GetName()}");
            using var reader = new StreamReader(stream, Encoding.UTF8);

            return reader.ReadToEndAsync();
        }

        public Stream GetScriptStream(string fileName)
        {
            return DataAssembly.GetManifestResourceStream(prefix + fileName)
                ?? throw new FileNotFoundException($"Unable to find {fileName} in {DataAssembly.GetName()}");
        }

        public IReadOnlyCollection<string> GetAllSqlFiles()
        {
            return DataAssembly
                .GetManifestResourceNames()
                .Where(IsSqlFile)
                .Select(StripPrefixFromFileName)
                .ToList();
        }

        private string StripPrefixFromFileName(string file) => file.Replace(prefix, string.Empty);
    }
}
