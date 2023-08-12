using Microsoft.Extensions.Configuration;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator;

public sealed class PublishOnlyApplication : IRunApplication
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
        var location = configuration.GetSection("PublishOnly:Location").Value ?? throw new InvalidOperationException("PublishOnly:Location empty");
        var branch = configuration.GetSection("Shields:BranchName").Value ?? throw new InvalidOperationException("Shields:BranchName empty");
        var version = configuration.GetSection("Shields:Version").Value ?? throw new InvalidOperationException("Shields:Version empty");
        var buildNumber = configuration.GetSection("Shields:BuildNumber").Value ?? throw new InvalidOperationException("Shields:BuildNumber empty");

        await Task.WhenAll
        (
            fileCreator.CreateApiFileAsync(location),
            fileCreator.CreateDataFilesAsync(location),
            fileCreator.CreateVersionInformationAsync(location, version, branch, buildNumber)
        );
    }
}