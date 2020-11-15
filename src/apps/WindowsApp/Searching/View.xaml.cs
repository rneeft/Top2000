#nullable enable

using Chroomsoft.Top2000.Features.Searching;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.Searching
{
    public sealed partial class View : Page, ISortGroupNameProvider
    {
        public View()
        {
            ViewModel = App.GetService<ViewModel>();
            this.InitializeComponent();
        }

        public ViewModel ViewModel { get; }

        public string GetNameForGroup(IGroup group)
        {
            return GetNameForGroupOrSortBy(group)
                ?? throw new NotImplementedException($"Group '{group.GetType()}' was not defined yet.");
        }

        public string GetNameForSort(ISort sort)
        {
            return GetNameForGroupOrSortBy(sort)
                ?? throw new NotImplementedException($"Display text for sort option '{sort.GetType()}' was not defined.");
        }

        public async Task OpenTrackDetailsAsync()
        {
            if (ViewModel.SelectedTrack is null) return;

            if (DetailsFrame.Content is TrackInformation.View view)
            {
                await view.LoadNewTrackAsync(ViewModel.SelectedTrack.Id);
            }
            else
            {
                DetailsFrame.Navigate(typeof(TrackInformation.View), NavigationData(), new SuppressNavigationTransitionInfo());
            }
        }

        public Visibility ShowWhenGrouped(GroupViewModel group)
        {
            return group.Value is GroupByNothing
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public Visibility HideWhenGrouped(GroupViewModel group)
        {
            return group.Value is GroupByNothing
                            ? Visibility.Visible
                            : Visibility.Collapsed;
        }

        private TrackInformation.NavigationData NavigationData()
        {
            return new TrackInformation.NavigationData
                (ViewModel.SelectedTrack?.Id ?? 1, ClearSelectedTrack);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.OnLoad(this);
        }

        private void ClearSelectedTrack()
        {
            ViewModel.SelectedTrack = null;
            DetailsFrame.Content = null;
        }

        private string? GetNameForGroupOrSortBy(object item)
        {
            switch (item)
            {
                case GroupByNothing _: return None.Text;
                case SortByArtist _:
                case GroupByArtist _: return Artist.Text;
                case SortByRecordedYear _:
                case GroupByRecordedYear _: return Year.Text;
                case SortByTitle _: return Title.Text;
                default:
                    return null;
            }
        }
    }
}
