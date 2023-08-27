using Chroomsoft.Top2000.Apps.Views.Overview.ByDate.SelectDateTimeGroup;
using Chroomsoft.Top2000.Apps.Views.TrackInformation;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByDate;

public partial class OverviewByDatePage : ContentPage, IQueryAttributable
{
    public OverviewByDatePage(OverviewByDateViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    public OverviewByDateViewModel ViewModel => (OverviewByDateViewModel)BindingContext;

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

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("SelectedDate"))
        {
            var selectedDateTime = (DateTime)query["SelectedDate"];
            await JumpToSelectedDateTime(selectedDateTime);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel.Listings.Count == 0)
        {
            await ViewModel.InitialiseViewModelAsync();
        }

        await JumpWhenTop2000IsOn();
    }

    private async Task JumpWhenTop2000IsOn()
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
            this.ToolbarItems.Remove(jumpToToday);
        }
    }

    private async Task JumpToSelectedDateTime(DateTime selectedDate)
    {
        var groupsBefore = ViewModel
            .Listings
            .Where(x => x.Key <= selectedDate);
        var group = groupsBefore.LastOrDefault();

        if (group is not null)
        {
            var firstGroup = group.FirstOrDefault();
            if (firstGroup is not null)
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

    private async void OpenTodayClick(object sender, EventArgs e)
    {
        await JumpToSelectedDateTime(DateTime.Now);
    }

    private async void OnListingSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 1)
        {
            var track = (TrackListing)e.CurrentSelection[0];
            await TrackInformationViewModel.NavigateAsync(track.TrackId, track.Title, track.Artist);
            listings.SelectedItem = null;
        }
    }

    private async void OnJumpGroupButtonClick(object sender, EventArgs e) => await NavigateSelectDateTimeGroupPage();

    private async void OnGroupHeaderClick(object sender, TappedEventArgs e) => await NavigateSelectDateTimeGroupPage();

    private async Task NavigateSelectDateTimeGroupPage()
    {
        var dict = new Dictionary<string, object>
        {
            {  "Dates", ViewModel.Dates }
        };

        await Shell.Current.GoToAsync(nameof(SelectDateTimeGroupPage), animate: true, dict);
    }
}