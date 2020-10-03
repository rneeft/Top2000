using Chroomsoft.Top2000.Features.AllEditions;
using MediatR;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    public sealed partial class YearOverviewPage : Page
    {
        public YearOverviewPage()
        {
            this.InitializeComponent();
        }

        public YearOverviewViewModel? ViewModel { get; private set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel ??= App.GetService<YearOverviewViewModel>();

            await ViewModel.LoadAllEditionsAsync();
        }
    }

    public class YearOverviewViewModel
    {
        private readonly IMediator mediator;

        public YearOverviewViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ObservableCollection<Edition> Editions { get; } = new ObservableCollection<Edition>();

        public async Task LoadAllEditionsAsync()
        {
            Editions.Clear();

            var alleditions = await mediator.Send(new AllEditionsRequest());

            foreach (var edition in alleditions)
                Editions.Add(edition);
        }
    }
}
