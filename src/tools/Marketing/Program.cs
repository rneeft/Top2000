using System.IO;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Tools.Marketing
{
    internal class Program
    {
        public static byte[] GetPictureAsBytes(string location)
        {
            using var pictureStream = File.Open(location, FileMode.Open);
            using var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        private static async Task Main(string[] args)
        {
            if (args.Length == 1)
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(args[0]);

            var builder = new PresentationBuilder();

            var languages = new ScreenshotSettings[]
            {
                new Dutch(),
                new English(),
                new French(),
            };

            foreach (var language in languages)
            {
                await new Screenshot(language).CreateAsync().ConfigureAwait(false);

                builder
                    .AddMainSlide(GetPictureAsBytes(language.LanguageShortName + "_main.png"), language.Main)
                    .AddInfoSlide(GetPictureAsBytes(language.LanguageShortName + "_date.png"), language.Date)
                    .AddInfoSlide(GetPictureAsBytes(language.LanguageShortName + "_search.png"), language.Search)
                    .AddInfoSlide(GetPictureAsBytes(language.LanguageShortName + "_editions.png"), language.Editions)
                    .AddInfoSlide(GetPictureAsBytes(language.LanguageShortName + "_darkmode.png"), language.DarkMode);
            }

            builder.Save(@"Marketing2.pptx");
            builder.SaveAsPictures();
        }
    }
}
