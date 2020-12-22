using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Features.Searching;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Apps.Searching
{
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

        public bool IsFlat
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public Track? SelectedTrack
        {
            get { return GetPropertyValue<Track?>(); }
            set
            {
                if (value != null && SelectedTrack?.Id != value?.Id)
                {
                    SetPropertyValue(value);
                }
            }
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

        public void OnActivate(ISortGroupNameProvider nameProvider)
        {
            if (GroupByOptions.Count == 0)
            {
                GroupByOptions.ClearAddRange(groupOptions.Select(x => new GroupViewModel(x, nameProvider.GetNameForGroup(x))));
                SortByOptions.ClearAddRange(sortOptions.Select(x => new SortViewModel(x, nameProvider.GetNameForSort(x))));
                GroupBy = GroupByOptions.First();
                SortBy = SortByOptions.First();
            }
        }

        public async Task ExceuteSearchAsync()
        {
            var request = new SearchTrackRequest(QueryText, SortBy.Value, GroupBy.Value);
            var result = await mediator.Send(request);

            Results.ClearAddRange(result);
            ResultsFlat.ClearAddRange(Results.SelectMany(x => x));

            ResultsCount = ResultsFlat.Count > 99 ? "100+" : "" + ResultsFlat.Count;
        }

        public void ReSortGroup()
        {
            if (GroupBy is null || SortBy is null) return;
            var resultFlat = Results.SelectMany(x => x).ToList();
            var sorted = SortBy.Value.Sort(resultFlat);
            ResultsFlat.ClearAddRange(sorted);

            var groupedAndSorted = GroupBy.Value.Group(sorted);
            Results.ClearAddRange(groupedAndSorted);

            IsGrouped = !(GroupBy.Value is GroupByNothing);
            IsFlat = !IsGrouped;
        }
    }
}
