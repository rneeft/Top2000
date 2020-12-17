using Chroomsoft.Top2000.Apps.Globalisation;
using Chroomsoft.Top2000.Apps.XamarinForms;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.Overview.Position
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage
    {
        public View()
        {
            BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Editions.Count == 0)
            {
                await ViewModel.InitialiseViewModelAsync();
            }
        }

        async private void OnSelectYearButtonClick(object sender, System.EventArgs e)
        {
            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await EditionsFlyout.TranslateTo(this.Width * -1, 0, 0);

            EditionsFlyout.IsVisible = true;
            await EditionsFlyout.TranslateTo(0, 0);
        }

        async private void OnJumpGroupButtonClick(object sender, System.EventArgs e)
        {
            var groups = ViewModel.Listings.Select(x => x.Key)
                                         .ToArray();

            var result = await DisplayActionSheet(AppResources.JumpToGroup, AppResources.Cancel, null, groups);

            if (!string.IsNullOrWhiteSpace(result) && result != AppResources.Cancel)
            {
                JumpIntoList(result);
            }
        }

        private void JumpIntoList(string groupElected)
        {
            var groupIndex = ViewModel.Listings.FindIndex(x => x.Key == groupElected);
            var group = ViewModel.Listings.Single(x => x.Key == groupElected);

            var position = group.First().Position;

            const int ShowGroup = 1;
            var index = position + groupIndex - ShowGroup;

            if (index < 0) index = 0;

            listings.ScrollTo(index, position: ScrollToPosition.Start, animate: false);
        }

        async private void NewEditionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedEdition is null) return;

            ViewModel.SelectedEditionYear = ViewModel.SelectedEdition.Year;

            var loadingTask = ViewModel.LoadAllListingsAsync();

            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await EditionsFlyout.TranslateTo(this.Width * -1, 0);
            this.EditionsFlyout.IsVisible = false;

            await loadingTask;

            JumpIntoList(ViewModel.Listings.First().Key);
        }
    }
}
