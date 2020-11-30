#nullable enable

using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    public sealed partial class View : Page
    {
        private const int GroupByPosition = 0;
        private const int GroupByDate = 1;

        private static PivotItem? datePivot;

        public View()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<ViewModel>();
            var updater = App.GetService<IGlobalUpdate>();
            updater.UpdateListsHandlerAsync = UpdateForNewList;
        }

        public ViewModel ViewModel { get; }

        public void OnEditionChanged()
        {
            if (ViewModel.SelectedEdition is null) return;

            var selectedPivotIndex = GroupByPivot.SelectedIndex;

            if (ViewModel.SelectedEdition.HasPlayDateAndTime && GroupByPivot.Items.Count == 1)
            {
                GroupByPivot.Items.Add(datePivot);
            }

            if (!ViewModel.SelectedEdition.HasPlayDateAndTime && GroupByPivot.Items.Count == 2)
            {
                GroupByPivot.Items.RemoveAt(1);
            }

            if (selectedPivotIndex == GroupByPivot.SelectedIndex)
            {
                NavigateToListing();
            }
        }

        public void NavigateToListing()
        {
            if (ViewModel.SelectedEdition is null) return;

            var navigationParam = new NavigationData(ViewModel.SelectedEdition, ShowTrackDetilsAsync)
            {
                SelectedTrackListing = ViewModel.SelectedTrackListing,
            };

            ListFrame.Navigate(ViewTypeForGroupByPivot(), navigationParam, new SuppressNavigationTransitionInfo());
        }

        private TrackInformation.NavigationData NavigationData()
        {
            return new TrackInformation.NavigationData
                (ViewModel.SelectedTrackListing?.TrackId ?? 1, ClearSelectedTrack);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

#pragma warning disable S2696 // Instance members should not write to "static" fields
            datePivot ??= (PivotItem)GroupByPivot.Items[1];
#pragma warning restore S2696 // Instance members should not write to "static" fields

            await ViewModel.LoadAllEditionsAsync();
            ViewModel.SelectedEdition ??= ViewModel.Editions.First();

            NavToTimeSlot.Visibility = ViewModel.IsTop2000OnAir
                ? Windows.UI.Xaml.Visibility.Visible
                : Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void NavigateToCurrentTimeSlot()
        {
            ViewModel.SelectedEdition = ViewModel.Editions.First();
            GroupByPivot.SelectedIndex = 1;
            if (ListFrame.Content is IListing listingPage)
            {
                listingPage.OpenCurrentDateAndTime();
            }
        }

        private async Task UpdateForNewList()
        {
            await ViewModel.LoadAllEditionsAsync();
            ViewModel.SelectedEdition = ViewModel.Editions.First();
            if (ViewModel.IsTop2000OnAir) NavigateToCurrentTimeSlot();
        }

        private void ClearSelectedTrack()
        {
            ViewModel.SelectedTrackListing = null;
            DetailsFrame.Content = null;
            if (ListFrame.Content is IListing listingPage)
            {
                listingPage.SetListing(null);
            }
        }

        private Type ViewTypeForGroupByPivot() => GroupByPivot.SelectedIndex switch
        {
            GroupByPosition => typeof(ListingPosition.View),
            GroupByDate => typeof(ListingDate.View),
            _ => throw new InvalidOperationException("Selected pivot item is not defined"),
        };

        private async Task ShowTrackDetilsAsync(TrackListing? listing)
        {
            if (listing is null) return;
            ViewModel.SelectedTrackListing = listing;

            if (DetailsFrame.Content is TrackInformation.View view)
            {
                await view.LoadNewTrackAsync(listing.TrackId);
            }
            else
            {
                DetailsFrame.Navigate(typeof(TrackInformation.View), NavigationData(), new SuppressNavigationTransitionInfo());
            }
        }
    }
}
