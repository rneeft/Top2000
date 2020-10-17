using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.TrackInformation;
using Chroomsoft.Top2000.WindowsApp.Common;
using Chroomsoft.Top2000.WindowsApp.ListingDate;
using Chroomsoft.Top2000.WindowsApp.ListingPosition;
using MediatR;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    public sealed partial class YearOverviewPage : Page, INotifyPropertyChanged
    {
        private const int GroupByPosition = 0;
        private const int GroupByDate = 1;
        private YearOverviewViewModel? viewModel;

        private PivotItem datePivot;

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

        public void OnEditionChanged()
        {
            var selectedPivotIndex = GroupByPivot.SelectedIndex;

            if (viewModel.SelectedEdition.HasPlayDateAndTime && GroupByPivot.Items.Count == 1)
            {
                GroupByPivot.Items.Add(datePivot);
            }

            if (!viewModel.SelectedEdition.HasPlayDateAndTime && GroupByPivot.Items.Count == 2)
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
            if (viewModel.SelectedEdition is null)
                return;

            var type = GroupByPivot.SelectedIndex == GroupByPosition
                ? typeof(ListingPositionPage)
                : typeof(ListingDatePage);

            ListFrame.Navigate(type, viewModel.SelectedEdition, new SuppressNavigationTransitionInfo());
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            datePivot = (PivotItem)GroupByPivot.Items[1];

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

        public Edition SelectedEdition
        {
            get { return GetPropertyValue<Edition>(); }
            set { SetPropertyValue(value); }
        }

        public TrackDetails TrackDetails
        {
            get { return GetPropertyValue<TrackDetails>(); }
            set { SetPropertyValue(value); }
        }

        public async Task LoadAllEditionsAsync()
        {
            Editions.AddRange(await mediator.Send(new AllEditionsRequest()));
            SelectedEdition = Editions.First();
        }
    }
}
