using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.Globalisation;
using Chroomsoft.Top2000.Apps.XamarinForms;
using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using MediatR;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.Overview
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage
    {
        private readonly NavigationShell.View shell;

        public View()
        {
            BindingContext = App.GetService<ViewModel>();
            shell = App.GetService<NavigationShell.View>();
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
            Shell.SetTabBarIsVisible(this, false);
            Shell.SetNavBarIsVisible(this, false);
            shell.IsDatesVisible = false;

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
                var groupIndex = ViewModel.Listings.FindIndex(x => x.Key == result);
                var group = ViewModel.Listings.Single(x => x.Key == result);

                var position = group.First().Position;

                const int ShowGroup = 1;
                var index = position + groupIndex - ShowGroup;

                if (index < 0) index = 0;

                listings.ScrollTo(index, position: ScrollToPosition.Start, animate: false);
            }
        }

        async private void NewEditionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedEdition is null) return;

            ViewModel.SelectedEditionYear = ViewModel.SelectedEdition.Year;

            await ViewModel.LoadAllListingsAsync();

            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            shell.IsDatesVisible = ViewModel.SelectedEdition.HasPlayDateAndTime;
            await EditionsFlyout.TranslateTo(this.Width * -1, 0);
            this.EditionsFlyout.IsVisible = false;
        }
    }

    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Listings = new ObservableGroupedList<string, TrackListing>();
            this.Editions = new ObservableList<Edition>();
        }

        public ObservableGroupedList<string, TrackListing> Listings { get; }

        public TrackListing? SelectedListing
        {
            get { return GetPropertyValue<TrackListing?>(); }
            set { SetPropertyValue(value); }
        }

        public ObservableList<Edition> Editions { get; }

        public Edition? SelectedEdition
        {
            get { return GetPropertyValue<Edition?>(); }
            set { SetPropertyValue(value); }
        }

        public int SelectedEditionYear
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public static string Position(TrackListing listing)
        {
            const int GroupSize = 100;

            if (listing.Position < 100) return "1 - 100";
            if (listing.Position >= 1900) return "1900 - 2000";

            var min = listing.Position / GroupSize * GroupSize;
            var max = min + GroupSize;

            return $"{min} - {max}";
        }

        public async Task InitialiseViewModelAsync()
        {
            var editions = await mediator.Send(new AllEditionsRequest());
            SelectedEdition = editions.First();
            SelectedEditionYear = SelectedEdition.Year;
            Editions.ClearAddRange(editions);

            await LoadAllListingsAsync();
        }

        public async Task LoadAllListingsAsync()
        {
            if (SelectedEdition is null) return;

            var listings = await mediator.Send(new AllListingsOfEditionRequest(SelectedEdition.Year));

            Listings.ClearAddRange(listings.GroupBy(Position));

            SelectedListing = null;
        }
    }
}
