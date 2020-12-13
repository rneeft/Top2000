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
        public View()
        {
            BindingContext = App.GetService<ViewModel>();

            InitializeComponent();
        }

        public static View MyView => App.GetService<View>();

        public ViewModel ViewModel => (ViewModel)BindingContext;

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Listings.Count == 0)
            {
                var edition = await ViewModel.LatestEditionAsync();
                await ViewModel.LoadListingForEditionAsync(edition);
            }
        }

        private async Task OnEditionSelected(Edition edition)
        {
            await ViewModel.LoadListingForEditionAsync(edition);

            var view = App.GetService<NavigationShell.View>();
            view.IsDatesVisible = edition.HasPlayDateAndTime;
        }

        async private void OnSelectYearButtonClick(object sender, System.EventArgs e)
        {
            var view = App.GetService<YearSelector.View>();
            await view.ShowAsModalDialog(2019, OnEditionSelected);
            await Navigation.PushModalAsync(new NavigationPage(view), animated: false);
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
    }

    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Listings = new ObservableGroupedList<string, TrackListing>();
        }

        public ObservableGroupedList<string, TrackListing> Listings { get; }

        public TrackListing? SelectedListing
        {
            get { return GetPropertyValue<TrackListing?>(); }
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

        public async Task LoadListingForEditionAsync(Edition edition)
        {
            var tracks = await mediator.Send(new AllListingsOfEditionRequest(edition.Year));
            var x = tracks.GroupBy(Position);
            Listings.ClearAddRange(x);

            SelectedListing = null;
            SelectedEditionYear = edition.Year;
        }

        public async Task<Edition> LatestEditionAsync()
        {
            var editions = await mediator.Send(new AllEditionsRequest());
            return editions.First();
        }
    }
}
