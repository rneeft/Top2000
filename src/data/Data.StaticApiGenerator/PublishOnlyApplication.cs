using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator
{
    public class PublishOnlyApplication : IRunApplication
    {
        private readonly IConfiguration configuration;
        private readonly IFileCreator fileCreator;

        public PublishOnlyApplication(IConfiguration configuration, IFileCreator fileCreator)
        {
            this.configuration = configuration;
            this.fileCreator = fileCreator;
        }

        public async Task RunAsync()
        {
            var location = configuration.GetSection("PublishOnly:Location").Value;
            var branch = configuration.GetSection("Shields:BranchName").Value;
            var version = configuration.GetSection("Shields:Version").Value;
            var buildNumber = configuration.GetSection("Shields:BuildNumber").Value;

            await Task.WhenAll
            (
                fileCreator.CreateApiFileAsync(location),
                fileCreator.CreateDataFilesAsync(location),
                fileCreator.CreateVersionInformationAsync(location, version, branch, buildNumber)
            ).ConfigureAwait(false);
        }
    }
}
