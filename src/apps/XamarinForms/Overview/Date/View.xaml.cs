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

        private static int FirstVisibleItemIndex { get; set; }

        public async Task ScrollToCorrectPositionAsync(int index, ScrollToPosition scrollToPosition = ScrollToPosition.Start)
        {
            var tries = 0;

            while (index != FirstVisibleItemIndex && tries < 6)
            {
                listings.ScrollTo(index, position: scrollToPosition, animate: false);

                tries++;

                await Task.Delay(200);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.GroupFlyout.IsVisible)
            {
                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                this.GroupFlyout.TranslateTo(this.Width * -1, 0);
                this.GroupFlyout.IsVisible = false;

                return true;
            }

            if (this.trackInformation.IsVisible)
            {
                CloseTrackInformationAsync();
                return true;
            }

            return base.OnBackButtonPressed();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Listings.Count == 0)
            {
                await ViewModel.InitialiseViewModelAsync();
            }

            await JumpWhenTop2000IsOn();
        }

        async private Task JumpWhenTop2000IsOn()
        {
            var first = ViewModel.Listings.First().Key;
            var last = ViewModel.Listings.Last().Key;
            var current = DateTime.Now;

            if (current > first && current < last)
            {
                await JumpToSelectedDateTime(current);
            }
            else
            {
                //this.ToolbarItems.Remove(jumpToToday);
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

            var item = e.CurrentSelection.ToList().FirstOrDefault();

            if (item != null)
            {
                await JumpToSelectedDateTime((DateTime)item);
            }
        }

        private async Task JumpToSelectedDateTime(DateTime selectedDate)
        {
            var tracksGrouped = ViewModel.Listings;
            var groupsBefore = tracksGrouped.Where(x => x.Key <= selectedDate);
            var group = groupsBefore.LastOrDefault();

            if (group != null)
            {
                var firstGroup = group.FirstOrDefault();
                if (firstGroup != null)
                {
                    var position = group.First().Position;
                    var totalTracks = ViewModel.Listings.SelectMany(x => x).Count();

                    const int ShowGroup = 1;
                    var index = totalTracks - position + groupsBefore.Count() - ShowGroup;

                    if (index < 0) index = 0;

                    await ScrollToCorrectPositionAsync(index);
                }
            }
        }

        private void Tracks_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            FirstVisibleItemIndex = e.FirstVisibleItemIndex;
        }

        async private void OpenTodayClick(object sender, EventArgs e)
        {
            var currentTime = DateTime.Now;
            var last = ViewModel.Listings.Last().Key;

            if (currentTime > last)
            {
                currentTime = ViewModel.Listings[12].Key;
            }

            await JumpToSelectedDateTime(currentTime);
        }

        async private void OnListingSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedListing is null) return;

            var infoTask = trackInformation.LoadTrackDetailsAsync(ViewModel.SelectedListing.TrackId, CloseTrackInformationAsync);

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await trackInformation.TranslateTo(this.Width * -1, 0, 0);

            trackInformation.IsVisible = true;
            await trackInformation.TranslateTo(0, 0);

            await infoTask;
        }

        private async Task CloseTrackInformationAsync()
        {
            ViewModel.SelectedListing = null;

            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await this.trackInformation.TranslateTo(this.Width * -1, 0);
            this.trackInformation.IsVisible = false;
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
