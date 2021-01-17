using System.Threading.Tasks;
using Xamarin.UITest;

namespace Chroomsoft.Top2000.Tools.Marketing
{
    public class Screenshot
    {
        private readonly IApp app;
        private readonly ScreenshotSettings settings;

        public Screenshot(ScreenshotSettings settings)
        {
            app = ConfigureApp.Android
                .InstalledApp("com.chroomsoft.top2000")
                .EnableLocalScreenshots()
                .StartApp();

            this.settings = settings;
        }

        public bool IsDarkModeActivated => app.Query("IsDarkModeOn")[0].Text == "True";

        public async Task CreateAsync()
        {
            app.WaitForElement("SelectEditionMenu");

            await SetupApplication().ConfigureAwait(false);
            SelectEditionsMenu();
            TrackInformation();
            AllEditions();
            DateScreen();
            await Search().ConfigureAwait(false);
            OviewViewDarkMode();
        }

        private async Task SetupApplication()
        {
            app.TapCoordinates(1258, 2583);
            app.WaitForElement("UseDarkModeSwitch");

            if (IsDarkModeActivated)
            {
                app.Tap("UseDarkModeSwitch");
            }

            app.Tap(settings.Language);
            await Task.Delay(300);
        }

        private void SelectEditionsMenu()
        {
            app.Tap(settings.OverviewName);
            app.WaitForElement("SelectEditionMenu");
            CreateScreenshot("main");
        }

        private void TrackInformation()
        {
            app.WaitForElement("Wish You Were Here");
            app.Tap("Wish You Were Here");
            app.WaitForElement("Pink Floyd (1975)");
            CreateScreenshot("info");
            app.Back();
        }

        private void AllEditions()
        {
            app.Tap("SelectEditionMenu");
            app.WaitForElement(c => c.Marked("SelectEditionLabel"));
            CreateScreenshot("editions");
            app.Back();
        }

        private void DateScreen()
        {
            app.WaitForElement(settings.DateName);
            app.Tap(settings.DateName);
            app.WaitForElement("JumpGroupDateToolbarItem");
            CreateScreenshot("date");
        }

        private async Task Search()
        {
            app.WaitForElement(settings.SearchName);
            app.Tap(settings.SearchName);
            app.WaitForElement(settings.SearchPlaceholder);
            app.Tap(settings.SearchPlaceholder);
            await Task.Delay(300);
            app.Query(x => x.Marked(settings.SearchPlaceholder).Invoke("setText", "Queen"));

            await Task.Delay(300);
            CreateScreenshot("search");
        }

        private void OviewViewDarkMode()
        {
            app.WaitForElement(x => x.Marked(settings.SettingsName));
            app.Tap(settings.SettingsName);
            app.Tap("UseDarkModeSwitch");

            app.Tap(settings.OverviewName);
            app.WaitForElement(c => c.Marked("SelectEditionMenu"));
            CreateScreenshot("darkmode");
        }

        private void CreateScreenshot(string name)
        {
            var screenshot = app.Screenshot(name);
            screenshot.CopyTo($"{settings.LanguageShortName}_{name}.png", overwrite: true);
            screenshot.Delete();
        }
    }
}
