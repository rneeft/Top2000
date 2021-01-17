using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace XamarinForms.UITests
{
    [TestFixture(Platform.Android)]
    public class CreateScreenshots
    {
        private IApp app;
        private Platform platform;

        public CreateScreenshots(Platform platform)
        {
            this.platform = platform;
        }

        public bool IsDarkModeActivated => app.Query("IsDarkModeOn")[0].Text == "True";

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public async Task Dutch()
        {
            app.WaitForElement("SelectEditionMenu");

            // open settings
            app.TapCoordinates(1258, 2583);
            app.WaitForElement("UseDarkModeSwitch");

            // make sure darkmode is off
            if (IsDarkModeActivated)
            {
                app.Tap("UseDarkModeSwitch");
            }

            app.Tap("Nederlands (Nederland)");
            await Task.Delay(300);
            app.Tap("Overzicht");

            app.WaitForElement("SelectEditionMenu");
            CreateScreenshot("nl_main");

            app.WaitForElement("Wish You Were Here");
            app.Tap("Wish You Were Here");
            app.WaitForElement("Pink Floyd (1975)");
            CreateScreenshot("nl_info");
            app.Back();

            app.Tap("SelectEditionMenu");
            app.WaitForElement(c => c.Marked("SelectEditionLabel"));
            CreateScreenshot("nl_selectEdition");
            app.Back();

            app.WaitForElement("Datum");
            app.Tap("Datum");
            app.WaitForElement("JumpGroupDateToolbarItem");
            CreateScreenshot("nl_dateView");

            app.WaitForElement("Zoeken");
            app.Tap("Zoeken");
            app.WaitForElement("Zoek in Top 2000");
            app.Tap("Zoek in Top 2000");
            await Task.Delay(300);
            app.Query(x => x.Marked("Zoek in Top 2000").Invoke("setText", "Queen"));

            await Task.Delay(300);
            CreateScreenshot("nl_search");

            app.WaitForElement(x => x.Marked("Instellingen"));
            app.Tap("Instellingen");
            app.Tap("UseDarkModeSwitch");

            app.Tap("Overzicht");
            app.WaitForElement(c => c.Marked("SelectEditionMenu"));
            CreateScreenshot("nl_darkmode");
        }

        [Test]
        public async Task English()
        {
            app.WaitForElement("SelectEditionMenu");

            // open settings
            app.TapCoordinates(1258, 2583);
            app.WaitForElement("UseDarkModeSwitch");

            // make sure darkmode is off
            if (IsDarkModeActivated)
            {
                app.Tap("UseDarkModeSwitch");
            }

            app.Tap("English (United Kingdom)");
            await Task.Delay(300);
            app.Tap("Overview");

            app.WaitForElement("SelectEditionMenu");
            CreateScreenshot("en_main");

            app.WaitForElement("Wish You Were Here");
            app.Tap("Wish You Were Here");
            app.WaitForElement("Pink Floyd (1975)");
            CreateScreenshot("en_info");
            app.Back();

            app.Tap("SelectEditionMenu");
            app.WaitForElement(c => c.Marked("SelectEditionLabel"));
            CreateScreenshot("en_selectEdition");
            app.Back();

            app.WaitForElement("Date");
            app.Tap("Date");
            app.WaitForElement("JumpGroupDateToolbarItem");
            CreateScreenshot("en_dateView");

            app.WaitForElement("Search");
            app.Tap("Search");
            app.WaitForElement("Search the Top 2000");
            app.Tap("Search the Top 2000");
            await Task.Delay(300);
            app.Query(x => x.Marked("Search the Top 2000").Invoke("setText", "Queen"));

            await Task.Delay(300);
            CreateScreenshot("en_search");

            app.WaitForElement(x => x.Marked("Settings"));
            app.Tap("Settings");
            app.Tap("UseDarkModeSwitch");

            app.Tap("Overview");
            app.WaitForElement(c => c.Marked("SelectEditionMenu"));
            CreateScreenshot("en_darkmode");
        }

        [Test]
        public async Task French()
        {
            app.WaitForElement("SelectEditionMenu");

            // open settings
            app.TapCoordinates(1258, 2583);
            app.WaitForElement("UseDarkModeSwitch");

            // make sure darkmode is off
            if (IsDarkModeActivated)
            {
                app.Tap("UseDarkModeSwitch");
            }

            app.Tap("");
            await Task.Delay(300);
            app.Tap("Aperçu");

            app.WaitForElement("SelectEditionMenu");
            CreateScreenshot("fr_main");

            app.WaitForElement("Wish You Were Here");
            app.Tap("Wish You Were Here");
            app.WaitForElement("Pink Floyd (1975)");
            CreateScreenshot("fr_info");
            app.Back();

            app.Tap("SelectEditionMenu");
            app.WaitForElement(c => c.Marked("SelectEditionLabel"));
            CreateScreenshot("fr_selectEdition");
            app.Back();

            app.WaitForElement("Date");
            app.Tap("Date");
            app.WaitForElement("JumpGroupDateToolbarItem");
            CreateScreenshot("fr_dateView");

            app.WaitForElement("Recherche");
            app.Tap("Recherche");
            app.WaitForElement("Rechercher le Top 2000");
            app.Tap("Rechercher le Top 2000");
            await Task.Delay(300);
            app.Query(x => x.Marked("Rechercher le Top 2000").Invoke("setText", "Queen"));

            await Task.Delay(300);
            CreateScreenshot("fr_search");

            app.WaitForElement(x => x.Marked("Paramètres"));
            app.Tap("Paramètres");
            app.Tap("UseDarkModeSwitch");

            app.Tap("Aperçu");
            app.WaitForElement(c => c.Marked("SelectEditionMenu"));
            CreateScreenshot("fr_darkmode");
        }

        private void CreateScreenshot(string name)
        {
            var path = Path.Combine(@"S:\", "screenshots");

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var file = Path.Combine(path, $"{name}.png");
            app.Screenshot(name).CopyTo(file, overwrite: true);
        }
    }
}
