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

            app.WaitForElement("Wish You Were Here");
            app.Tap("Wish You Were Here");
            await Task.Delay(300);
            app.Screenshot("Track");
            app.Back();

            app.Tap("SelectEditionMenu");
            app.WaitForElement(c => c.Marked("SelectEditionLabel"));
            app.Screenshot("SelectEdition.");
            app.Back();

            app.WaitForElement("JumpGroupToolbarItem");
            app.Tap("JumpGroupToolbarItem");
            await Task.Delay(300);
            app.Screenshot("Jump");
            app.Back();

            app.WaitForElement("Datum");
            app.Tap("Datum");
            app.WaitForElement("JumpGroupDateToolbarItem");
            app.Screenshot("Datum");

            app.WaitForElement("Zoeken");
            app.Tap("Zoeken");
            app.WaitForElement("Zoek in Top 2000");
            app.Tap("Zoek in Top 2000");
            await Task.Delay(300);
            app.Query(x => x.Marked("Zoek in Top 2000").Invoke("setText", "Queen"));

            await Task.Delay(300);
            app.Screenshot("Search");

            app.WaitForElement(x => x.Marked("Instellingen"));
            app.Tap("Instellingen");
            app.Tap("UseDarkModeSwitch");

            app.Tap("Overzicht");
            app.WaitForElement(c => c.Marked("SelectEditionMenu"));
            app.Screenshot("DarkMode");
        }
    }
}
