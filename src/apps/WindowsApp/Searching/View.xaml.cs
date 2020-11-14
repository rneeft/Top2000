#nullable enable

using Chroomsoft.Top2000.Features.Searching;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (DetailsGrid.Content is TrackInformation.View view)
            {
                await view.LoadNewTrackAsync(ViewModel.SelectedTrack.Id);
            }
            else
            {
                DetailsGrid.Navigate(typeof(TrackInformation.View), ViewModel.SelectedTrack.Id, new SuppressNavigationTransitionInfo());
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.OnLoad(this);
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

        private void OnDetailsPageNavigated(object sender, NavigationEventArgs e)
        {
        }
    }

    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;
        private readonly IEnumerable<IGroup> groupOptions;
        private readonly IEnumerable<ISort> sortOptions;

        public ViewModel(IMediator mediator, IEnumerable<IGroup> groupOptions, IEnumerable<ISort> sortOptions)
        {
            this.mediator = mediator;
            this.groupOptions = groupOptions;
            this.sortOptions = sortOptions;
            this.GroupByOptions = new ObservableList<GroupViewModel>();
            this.SortByOptions = new ObservableList<SortViewModel>();
            this.Results = new ObservableGroupedList<string, Track>();
            this.ResultsFlat = new ObservableList<Track>();
        }

        public string QueryText
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public GroupViewModel GroupBy
        {
            get { return GetPropertyValue<GroupViewModel>(); }
            set { SetPropertyValue(value); }
        }

        public SortViewModel SortBy
        {
            get { return GetPropertyValue<SortViewModel>(); }
            set { SetPropertyValue(value); }
        }

        public bool IsGrouped
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public Track? SelectedTrack
        {
            get { return GetPropertyValue<Track?>(); }
            set { SetPropertyValue(value); }
        }

        public string ResultsCount
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public ObservableList<GroupViewModel> GroupByOptions { get; }

        public ObservableList<SortViewModel> SortByOptions { get; }

        public ObservableGroupedList<string, Track> Results { get; }

        public ObservableList<Track> ResultsFlat { get; }

        public void OnLoad(ISortGroupNameProvider nameProvider)
        {
            GroupByOptions.AddRange(groupOptions.Select(x => new GroupViewModel(x, nameProvider.GetNameForGroup(x))));
            SortByOptions.AddRange(sortOptions.Select(x => new SortViewModel(x, nameProvider.GetNameForSort(x))));
            GroupBy = GroupByOptions.First();
            SortBy = SortByOptions.First();
        }

        public async Task ExceuteSearchAsync()
        {
            var request = new SearchTrackRequest(QueryText, SortBy.Value, GroupBy.Value);
            var result = await mediator.Send(request);

            Results.AddRange(result);
            ResultsFlat.AddRange(Results.SelectMany(x => x));

            ResultsCount = ResultsFlat.Count > 99 ? "100+" : "" + ResultsFlat.Count;
        }

        public void ReSortGroup()
        {
            var resultFlat = Results.SelectMany(x => x).ToList();
            var sorted = SortBy.Value.Sort(resultFlat);
            ResultsFlat.AddRange(sorted);

            var groupedAndSorted = GroupBy.Value.Group(sorted);
            Results.AddRange(groupedAndSorted);

            IsGrouped = !(GroupBy.Value is GroupByNothing);
        }
    }
}
