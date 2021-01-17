using System;
using System.IO;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Tools.Marketing
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            if (args.Length == 1)
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(args[0]);

            var dutch = new ScreenshotSettings
            {
                Language = "Nederlands (Nederland)",
                LanguageShortName = "nl",
                SettingsName = "Instellingen",
                OverviewName = "Overzicht",
                SearchName = "Zoeken",
                SearchPlaceholder = "Zoek in Top 2000",
                DateName = "Datum",
                Main = "Bekijk de+Top 2000+met helder+overzicht+van alle+edities",
                Date = "Bekijk de lijst per uur en+navigeer makkelijk naar het+huidige uur",
                Search = "Vind wat je zoekt+op titel en artiest",
                Info = "Uitgebreide informatie+in één handig overzicht",
                Editions = "Alle overzichten+vanaf 1999 tot en met nu",
                DarkMode = "Kies voor de+donkere modus",
            };

            var english = new ScreenshotSettings
            {
                Language = "English (United Kingdom)",
                LanguageShortName = "en",
                SettingsName = "Settings",
                OverviewName = "Overview",
                SearchName = "Search",
                SearchPlaceholder = "Search the Top 2000",
                DateName = "Date",
                Main = "Experience the+Top 2000+with clear+overview+of all the+editions",
                Date = "View the list per hour and+navigate easily to the+current moment",
                Search = "Search and find+by title and artist",
                Info = "Additional information in+one clear overview",
                Editions = "All the editions included+since 1999",
                DarkMode = "Choose the+dark mode",
            };

            var french = new ScreenshotSettings
            {
                Language = "Français (France)",
                LanguageShortName = "fr",
                SettingsName = "Paramètres",
                OverviewName = "Aperçu",
                SearchName = "Recherche",
                SearchPlaceholder = "Rechercher le Top 2000",
                DateName = "Date",
                Main = "Découvrez le+Top 2000 avec+clair aperçu+de tous+les ans",
                Date = "Voir la liste par heure et+naviguer facilement jusqu'à+l'heure actuelle",
                Search = "Rechercher+par titre et artiste",
                Info = "Informations supplémentaires+pour un clair aperçu",
                Editions = "Toutes les listes de 1999+à cette année ci",
                DarkMode = "Choisissez le mode sombre",
            };

            var builder = new PresentationBuilder();
            
         //   await new Screenshot(dutch).CreateAsync().ConfigureAwait(false);

            builder
                .AddMainSlide(GetPictureAsBytes(dutch.LanguageShortName + "_main.png"), dutch.Main)
                .AddInfoSlide(GetPictureAsBytes(dutch.LanguageShortName + "_date.png"), dutch.Date)
                .AddInfoSlide(GetPictureAsBytes(dutch.LanguageShortName + "_search.png"), dutch.Search)
                .AddInfoSlide(GetPictureAsBytes(dutch.LanguageShortName + "_editions.png"), dutch.Editions)
                .AddInfoSlide(GetPictureAsBytes(dutch.LanguageShortName + "_darkmode.png"), dutch.DarkMode);

         //   await new Screenshot(english).CreateAsync().ConfigureAwait(false);

            builder
              .AddMainSlide(GetPictureAsBytes(english.LanguageShortName + "_main.png"), english.Main)
              .AddInfoSlide(GetPictureAsBytes(english.LanguageShortName + "_date.png"), english.Date)
              .AddInfoSlide(GetPictureAsBytes(english.LanguageShortName + "_search.png"), english.Search)
              .AddInfoSlide(GetPictureAsBytes(english.LanguageShortName + "_editions.png"), english.Editions)
              .AddInfoSlide(GetPictureAsBytes(english.LanguageShortName + "_darkmode.png"), english.DarkMode);

        //    await new Screenshot(french).CreateAsync().ConfigureAwait(false);

            builder
              .AddMainSlide(GetPictureAsBytes(french.LanguageShortName + "_main.png"), french.Main)
              .AddInfoSlide(GetPictureAsBytes(french.LanguageShortName + "_date.png"), french.Date)
              .AddInfoSlide(GetPictureAsBytes(french.LanguageShortName + "_search.png"), french.Search)
              .AddInfoSlide(GetPictureAsBytes(french.LanguageShortName + "_editions.png"), french.Editions)
              .AddInfoSlide(GetPictureAsBytes(french.LanguageShortName + "_darkmode.png"), french.DarkMode);

            builder.Save(@"Marketing2.pptx");
            builder.SaveAsPictures();



        }

        public static byte[] GetPictureAsBytes(string location)
        {
            using var pictureStream = File.Open(location, FileMode.Open);
            using var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
