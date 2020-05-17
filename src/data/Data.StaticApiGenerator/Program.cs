using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var services = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .AddSingleton<ITop2000AssemblyData, Top2000Data>()
                .AddSingleton<ITransformSqlFiles, SqlFileTransformer>()
                .AddSingleton<IFileCreator, FileCreator>()
                .AddSingleton<IConfiguration>(configuration)
                ;

            var publishOnlySection = configuration.GetSection("PublishOnly");

            if (publishOnlySection.Exists())
            {
                services.AddSingleton<IRunApplication, PublishOnlyApplication>();
            }
            else
            {
                services.AddSingleton<IRunApplication, WebServiceApplication>();
            }

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            var application = serviceProvider.GetService<IRunApplication>();
            await application.RunAsync();
        }
    }

    public interface IRunApplication
    {
        Task RunAsync();
    }

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

            await Task.WhenAll
            (
                fileCreator.CreateApiFileAsync(location),
                fileCreator.CreateDataFilesAsync(location)
            );
        }
    }
}
