using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator
{
    public interface IFileCreator
    {
        Task CreateDataFilesAsync(string location);

        Task CreateApiFileAsync(string location);

        Task CreateVersionInformationAsync(string location, string version, string branchName, string buildNumber);
    }

    public class FileCreator : IFileCreator
    {
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly ILogger<FileCreator> logger;
        private readonly ITransformSqlFiles transformer;
        private readonly ITop2000AssemblyData top2000Data;

        public FileCreator(ILogger<FileCreator> logger, ITransformSqlFiles transformer, ITop2000AssemblyData top2000Data)
        {
            this.logger = logger;
            this.transformer = transformer;
            this.top2000Data = top2000Data;
        }

        public async Task CreateDataFilesAsync(string location)
        {
            var toUpload = top2000Data
                .GetAllSqlFiles()
                .ToList();

            foreach (var file in toUpload)
            {
                logger.LogInformation("Saving {file} to disk", file);

                var contents = await top2000Data.GetScriptContentAsync(file).ConfigureAwait(false);
                var path = Path.Combine(location, "data");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fileName = Path.Combine(path, file);

                await File.WriteAllTextAsync(fileName, contents).ConfigureAwait(false);
            }
        }

        public async Task CreateApiFileAsync(string location)
        {
            var versions = transformer.Transform();

            foreach (var version in versions)
            {
                logger.LogInformation("Saving version {version} to disk", version.Version);

                var path = Path.Combine(location, "api", "versions", version.Version);
                var json = JsonConvert.SerializeObject(version.Upgrades.Select(x => x.FileName));

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fileName = Path.Combine(path, "upgrades");

                await File.WriteAllTextAsync(fileName, json).ConfigureAwait(false);
            }
        }

        public async Task CreateVersionInformationAsync(string location, string version, string branchName, string buildNumber)
        {
            var shields = new[]
            {
                new ShieldsIO("Branch", branchName),
                new ShieldsIO("Version", version),
                new ShieldsIO("Build", buildNumber)
            };

            var path = Path.Combine(location, "shields");

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            foreach (var shield in shields)
            {
                var fileName = Path.Combine(path, shield.Label + ".json");
                var json = JsonConvert.SerializeObject(shield, serializerSettings);

                await File.WriteAllTextAsync(fileName, json).ConfigureAwait(false);
            }
        }

        private class ShieldsIO
        {
            public ShieldsIO(string label, string message)
            {
                this.Label = label;
                Message = message;
            }

            public int SchemaVersion { get; } = 1;

            public string Label { get; }

            public string Message { get; }
        }
    }
}
