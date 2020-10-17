using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.Features.TrackInformation;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

    public class YearOverviewViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public YearOverviewViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ObservableList<Edition> Editions { get; } = new ObservableList<Edition>();

        public ObservableList<ListingGroup> Listings { get; } = new ObservableList<ListingGroup>();

        public Edition SelectedEdition
        {
            get { return GetPropertyValue<Edition>(); }
            set { SetPropertyValue(value); }
        }

        public TrackListing SelectedListing
        {
            get { return GetPropertyValue<TrackListing>(); }
            set { SetPropertyValue(value); }
        }

        public TrackDetails TrackDetails
        {
            get { return GetPropertyValue<TrackDetails>(); }
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

        public async Task LoadAllEditionsAsync()
        {
            Editions.AddRange(await mediator.Send(new AllEditionsRequest()));
            SelectedEdition = Editions.First();
        }

        public async Task ShowListingForEditionAsync()
        {
            var tracks = (await mediator.Send(new AllListingsOfEditionRequest(SelectedEdition.Year)))
                .GroupBy(Position)
                .Select(x => new ListingGroup(x))
                .ToList();

            Listings.AddRange(tracks);
        }

        public async Task ShowListingDetailsAsync()
        {
            TrackDetails = await mediator.Send(new TrackInformationRequest(SelectedListing.TrackId));
        }

        
    }

    public class ListingGroup : List<TrackListing>, IGrouping<string, TrackListing>
    {
        public ListingGroup(IGrouping<string, TrackListing> group) : base(group)
        {
            this.Key = group.Key;
        }

        public string Key { get; }
        

    }

    public class ObservableGroupedList<TKey, TItem> : ObservableList<IGrouping<TKey, TItem>> { }

    public class ObservableList<TItem> : Collection<TItem>, INotifyCollectionChanged
    {
        private static readonly NotifyCollectionChangedEventArgs EventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void AddRange(IEnumerable<TItem> items)
        {
            this.Clear();

            items.ToList().ForEach(Add);

            CollectionChanged?.Invoke(this, EventArgs);
        }
    }
}
