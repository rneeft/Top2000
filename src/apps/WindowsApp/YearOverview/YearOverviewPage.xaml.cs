using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using MediatR;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    public sealed partial class YearOverviewPage : Page, INotifyPropertyChanged
    {
        private YearOverviewViewModel? viewModel;

        public YearOverviewPage()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public YearOverviewViewModel? ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel ??= App.GetService<YearOverviewViewModel>();
            await ViewModel.LoadAllEditionsAsync();
        }
    }

    public class YearOverviewViewModel : INotifyPropertyChanged
    {
        private readonly IMediator mediator;

        public YearOverviewViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Edition> Editions { get; } = new ObservableCollection<Edition>();

        public ObservableCollection<IGrouping<string, TrackListing>> Tracks { get; } = new ObservableCollection<IGrouping<string, TrackListing>>();

        public async Task LoadAllEditionsAsync()
        {
            Editions.Clear();

            var alleditions = await mediator.Send(new AllEditionsRequest());
            foreach (var edition in alleditions)
                Editions.Add(edition);

            Tracks.Clear();
            var alltracks =

           (await mediator.Send(new AllListingsOfEditionRequest(2019)))
              .GroupBy(TrackPositionGroup.GroupByPosition)
              .ToList();

            foreach (var track in alltracks)
                Tracks.Add(track);
        }

        public async Task ShowListingForEdition(Edition edition)
        {
            Tracks.Clear();
            var alltracks = (await mediator.Send(new AllListingsOfEditionRequest(edition.Year)))
              .GroupBy(TrackPositionGroup.GroupByPosition)
              .ToList();

            foreach (var track in alltracks)
                Tracks.Add(track);
        }
    }

    public class TrackPositionGroup : List<TrackListing>, IGrouping<string, TrackListing>
    {
        private TrackPositionGroup(string key) : base() => Key = key;

        public string Key { get; }

        public static string GroupByPosition(TrackListing arg)
        {
            if (arg.Position < 100) return "1 - 99";
            if (arg.Position >= 1900) return "1900 - 2000";

            const int GroupSize = 100;

            var minimum = arg.Position / GroupSize * GroupSize;
            var maximum = minimum + GroupSize - 1;

            return $"{minimum} - {maximum}";
        }

        public static TrackPositionGroup SelectGroup(IGrouping<string, TrackListing> group)
        {
            return new TrackPositionGroup(group.Key);
        }
    }
}
