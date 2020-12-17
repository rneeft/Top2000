using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.XamarinForms;
using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.Overview.Date
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
            if (ViewModel.Listings.Count == 0)
            {
                await ViewModel.InitialiseViewModelAsync();
            }
        }

        async private void OnJumpGroupButtonClick(object sender, EventArgs e)
        {
            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await GroupFlyout.TranslateTo(this.Width * -1, 0, 0);

            GroupFlyout.IsVisible = true;
            await GroupFlyout.TranslateTo(0, 0);
        }

        async private void OnGroupSelected(object sender, SelectionChangedEventArgs e)
        {
            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await GroupFlyout.TranslateTo(this.Width * -1, 0);
            this.GroupFlyout.IsVisible = false;
        }
    }

    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Listings = new ObservableGroupedList<DateTime, TrackListing>();
            this.Dates = new ObservableGroupedList<DateTime, DateTime>();
        }

        public ObservableGroupedList<DateTime, TrackListing> Listings { get; }

        public ObservableGroupedList<DateTime, DateTime> Dates { get; }

        public int SelectedEditionYear
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public TrackListing? SelectedListing
        {
            get { return GetPropertyValue<TrackListing?>(); }
            set { SetPropertyValue(value); }
        }

        public static DateTime LocalPlayDateAndTime(TrackListing listing) => listing.LocalPlayDateAndTime;

        public async Task InitialiseViewModelAsync()
        {
            var editions = await mediator.Send(new AllEditionsRequest());
            SelectedEditionYear = editions.First().Year;

            await LoadAllListingsAsync();
        }

        public async Task LoadAllListingsAsync()
        {
            var tracks = await mediator.Send(new AllListingsOfEditionRequest(SelectedEditionYear));
            var listings = tracks
                .OrderByDescending(x => x.Position)
                .GroupBy(LocalPlayDateAndTime);

            var dates = listings
                .Select(x => x.Key)
                .GroupBy(LocalPlayDate);

            Listings.ClearAddRange(listings);
            Dates.ClearAddRange(dates);
        }

        private DateTime LocalPlayDate(DateTime arg) => arg.Date;
    }
}
