#nullable enable

using Chroomsoft.Top2000.Features.AllListingsOfEdition;
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

        private PivotItem? datePivot;

        public View()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<ViewModel>();
        }

        public ViewModel ViewModel { get; }

        public void OnEditionChanged()
        {
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

        public void ClearSelectedTrack()
        {
            ViewModel.SelectedTrackListing = null;
            DetailsFrame.Content = null;
        }

        private TrackInformation.NavigationData NavigationData()
        {
            return new TrackInformation.NavigationData
                (ViewModel.SelectedTrackListing?.TrackId ?? 1, ClearSelectedTrack);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            datePivot = (PivotItem)GroupByPivot.Items[1];

            await ViewModel.LoadAllEditionsAsync();
            ViewModel.SelectedEdition = ViewModel.Editions.First();
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
