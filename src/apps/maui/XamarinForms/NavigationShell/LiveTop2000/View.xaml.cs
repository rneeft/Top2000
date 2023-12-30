using Chroomsoft.Top2000.Apps.AskForReview;
using Chroomsoft.Top2000.Apps.Globalisation;
using Chroomsoft.Top2000.Apps.XamarinForms;
using System.Threading.Tasks;


using Microsoft.Maui.Controls.Xaml;

namespace Chroomsoft.Top2000.Apps.NavigationShell.LiveTop2000
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : Shell, IMainShell
    {
        public View()
        {
            InitializeComponent();

#pragma warning disable 4014
            Task.Run(async () =>
            {
                await Task.Delay(5 * 1000);
                CheckForReviewAsync();
            }).ConfigureAwait(false);
#pragma warning restore 4014
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

        public void CheckForReviewAsync()
        {
            var reviews = App.GetService<IAskForReview>();

            if (reviews.MustAskForReview())
            {
                var strings = Translator.Instance;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var rateIt = await DisplayAlert(strings["RateTitle"], strings["RateText"], strings["RateIt"], strings["RateNotNow"]);

                    if (rateIt)
                    {
                        reviews.AskForReview();
                    }
                });
            }
        }
    }
}
