using Chroomsoft.Top2000.Features;
using Chroomsoft.Top2000.Data.ClientDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Reflection;
using System.Text;
using System;
using System.Threading.Tasks;
using System.IO;

namespace Chroomsoft.Top2000.Data.LocalDb
{
    public static class Program
    {
        private static IServiceProvider? serviceProvider;

        public static IServiceProvider ServiceProvider
        {
            get
            {
                return serviceProvider ??
                    throw new InvalidOperationException("Application isn't booted yet");
            }
            set
            {
                serviceProvider = value;
            }
        }

        public static T GetService<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();

        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var services = new ServiceCollection()
                .AddFeatures()
                .AddOfflineClientDatabase(new DirectoryInfo(Directory.GetCurrentDirectory()))
                .AddLogging(configure => configure.AddConsole())
                .AddSingleton<IConfiguration>(configuration)
                ;

            serviceProvider = services.BuildServiceProvider();

            await EnsureDatabaseIsCreatedAsync();

            var request = new Features.AllListingsOfEdition.AllListingsOfEditionRequest(2020);
            var mediator = GetService<IMediator>();

            var allListings = await mediator.Send(request);

            var htmlTemplate = ResourceFiles
                .Get("HtmlTemplate.html")
                .AsString();

            var builder = new StringBuilder();

            foreach (var listing in allListings)
            {
                builder.AppendLine(htmlTemplate
                    .Replace("..position..", listing.Position.ToString())
                    .Replace("..title..", listing.Title.ToString())
                    .Replace("..artist..", listing.Artist.ToString())
                    );
            }

            var html = builder.ToString();
            File.WriteAllText("content.html", html);
        }

        public static Task EnsureDatabaseIsCreatedAsync()
        {
            var databaseGen = GetService<IUpdateClientDatabase>();
            var top2000 = GetService<Top2000AssemblyDataSource>();

            return databaseGen.RunAsync(top2000);
        }
    }

    public class ResourceFiles
    {
        private readonly string name;

        private ResourceFiles(string name)
        {
            this.name = name;
        }

        public static ResourceFiles Get(string name) => new ResourceFiles(name);

        public Stream AsStream()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Web.StaticWebContentGenerator.{name}");

            if (stream is null)
            {
                var files = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                throw new FileNotFoundException($"name was not found, the following files are available: {string.Join(Environment.NewLine, files)}");
            }

            return stream;
        }

        public byte[] AsBytes()
        {
            using var stream = AsStream();
            using var memoryStream = new MemoryStream();

            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public string AsString()
        {
            using var stream = AsStream();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}