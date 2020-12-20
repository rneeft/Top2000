using Chroomsoft.Top2000.Apps.Globalisation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.NavigationShell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : Shell
    {
        public View()
        {
            InitializeComponent();
        }

        public void SetTitles()
        {
            var strings = Translator.Instance;

            OverviewTab.Title = strings["Overview"];
            ViewByDateTab.Title = strings["ViewByDate"];
            SearchTab.Title = strings["Search"];
            SettingsTab.Title = strings["Settings"];

            GeneralTab.Title = strings["General"];
            PrivacyTab.Title = strings["Privacy"];
            ThirdPartyTab.Title = strings["ThirdParty"];
            AboutTab.Title = strings["About"];

            PrivacyTab.IsVisible = false;
            ThirdPartyTab.IsVisible = false;
            AboutTab.IsVisible = false;

            PrivacyTab.IsVisible = true;
            ThirdPartyTab.IsVisible = true;
            AboutTab.IsVisible = true;
        }
    }
}
