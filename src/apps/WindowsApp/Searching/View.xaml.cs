using Chroomsoft.Top2000.Features.Searching;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.Searching
{
    public sealed partial class View : Page
    {
        public View()
        {
            ViewModel = App.GetService<ViewModel>();
            this.InitializeComponent();
        }

        public ViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void OnDetailsPageNavigated(object sender, NavigationEventArgs e)
        {

        }
    }

    public interface IGroup
    {
        string Strategy { get; }

        IEnumerable<IGrouping<string, Track>> Group(IEnumerable<Track> tracks);
    }

    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator, IGroup[] groupOptions, ISort[] sortOptions)
        {
            this.mediator = mediator;
        }

        public string QueryText
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public IGroup GroupBy
        {
            get { return GetPropertyValue<IGroup>(); }
            set { SetPropertyValue(value); }
        }

        public ISort SortBy
        {
            get { return GetPropertyValue<ISort>(); }
            set { SetPropertyValue(value); }
        }
    }
}
