using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator
{
    public class WebServiceApplication : IRunApplication
    {
        private readonly IFileCreator fileCreator;

        public WebServiceApplication(IFileCreator fileCreator)
        {
            this.fileCreator = fileCreator;
        }

        public async Task RunAsync()
        {
            var location = Path.Combine("wwwroot");

            await Task.WhenAll
            (
                fileCreator.CreateApiFileAsync(location),
                fileCreator.CreateDataFilesAsync(location)
            ).ConfigureAwait(false);

            var host = new WebHostBuilder()
                   .UseKestrel()
                   .Configure(app =>
                   {
                       app.UseDirectoryBrowser();
                       app.UseStaticFiles(new StaticFileOptions
                       {
                           ServeUnknownFileTypes = true,
                           DefaultContentType = "text/json"
                       });
                   })
                   .UseUrls("https://localhost:2000")
                   .Build();

            await host.RunAsync().ConfigureAwait(false);
        }
    }
}
