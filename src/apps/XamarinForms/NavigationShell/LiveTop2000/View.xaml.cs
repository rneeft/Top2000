using Chroomsoft.Top2000.Apps.Globalisation;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.NavigationShell.LiveTop2000
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : Shell, IMainShell
    {
        public View()
        {
            InitializeComponent();
        }

        public bool IsViewForWhenTop2000IsLive => true;

        public void SetTitles()
        {
            var strings = Translator.Instance;

            OverviewTab.Title = strings["Overview"];
            ViewByDateTab.Title = "Live!";
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
