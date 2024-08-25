using Microsoft.Extensions.Configuration;

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
            var location = configuration.GetSection("PublishOnly:Location").Value ?? throw new InvalidOperationException("Unable to find 'PublishOnly:Location' setting");
            var branch = configuration.GetSection("Shields:BranchName").Value ?? throw new InvalidOperationException("Unable to find 'Shields:BranchName' setting");
            var version = configuration.GetSection("Shields:Version").Value ?? throw new InvalidOperationException("Unable to find 'Shields:Version' setting");
            var buildNumber = configuration.GetSection("Shields:BuildNumber").Value ?? throw new InvalidOperationException("Unable to find 'Shields:BuildNumber' setting");

            await Task.WhenAll
            (
                fileCreator.CreateApiFileAsync(location),
                fileCreator.CreateDataFilesAsync(location),
                fileCreator.CreateVersionInformationAsync(location, version, branch, buildNumber)
            ).ConfigureAwait(false);
        }
    }
}
