using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using Xamarin.Essentials;

namespace WindowsApp
{
    public class AppHostBuilder
    {
        public IHostBuilder CreateDefaultAppHostBuilder()
        {
            return new HostBuilder()
                .ConfigureHostConfiguration(ConfigureConfiguration);
        }

        private static string SaveAppSettingsToLocalDisk()
        {
            var filename = "appsettings.json";
            var assembly = Assembly.GetExecutingAssembly();
            var appsettingsFile = assembly.GetName().Name + "." + filename;

            using var resFilestream = assembly.GetManifestResourceStream(appsettingsFile)
                ?? throw new FileNotFoundException($"Unable to find {filename} in {assembly.GetName()}");

            var localPath = Path.Combine(FileSystem.CacheDirectory, filename);

            using var stream = File.Create(localPath);
            resFilestream.CopyTo(stream);

            return localPath;
        }

        private void ConfigureConfiguration(IConfigurationBuilder builder)
        {
            var fullConfig = SaveAppSettingsToLocalDisk();

            builder.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
            builder.AddJsonFile(fullConfig);
        }
    }
}
