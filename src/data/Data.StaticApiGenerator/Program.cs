using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator
{
    public static class Program

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
            await application.RunAsync().ConfigureAwait(false);
        }
    }
}
