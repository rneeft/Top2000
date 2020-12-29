using NUnit.Framework;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace XamarinForms.UITests
{
    [TestFixture(Platform.Android)]
    public class Tests
    {
        private IApp app;
        private Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public async Task WelcomeTextIsDisplayed()
        {
            app.WaitForElement(c => c.Marked("SelectEditionMenu"));
            app.Screenshot("Start.");

            app.Tap("SelectEditionMenu");
            app.WaitForElement(c => c.Marked("SelectEditionLabel"));
            app.Screenshot("SelectEdition.");
            app.Back();

            app.WaitForElement("JumpGroupToolbarItem");
            app.Tap("JumpGroupToolbarItem");
            await Task.Delay(500);
            app.Screenshot("Jump");
            app.Back();

            app.WaitForElement(x => x.Marked("Instellingen"));
            app.Tap("Instellingen");
            app.Tap("UseDarkModeSwitch");
            app.Screenshot("SettingsDark.");

            app.Tap("Overzicht");
            app.WaitForElement(c => c.Marked("SelectEditionMenu"));
            app.Screenshot("DarkMode");
        }
    }
}
