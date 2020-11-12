using Chroomsoft.Top2000.Features.Searching;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.Searching
{
    public sealed partial class View : Page, IGroupNameProvider, ISortNameProvider
    {
        public View()
        {
            ViewModel = App.GetService<ViewModel>();
            this.InitializeComponent();
        }

        public ViewModel ViewModel { get; }

        public string GroupByName(IGroup group)
        {
            throw new System.NotImplementedException();
        }

        public string SortByName(ISort sort)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void OnDetailsPageNavigated(object sender, NavigationEventArgs e)
        {
            ViewModel.OnLoad(this, this);
        }
    }

    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;
        private readonly IGroup[] groupOptions;
        private readonly ISort[] sortOptions;

        public ViewModel(IMediator mediator, IGroup[] groupOptions, ISort[] sortOptions)
        {
            this.mediator = mediator;
            this.groupOptions = groupOptions;
            this.sortOptions = sortOptions;
            this.GroupByOptions = new ObservableList<GroupViewModel>();
            this.SortByOptions = new ObservableList<SortViewModel>();
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

        public ObservableList<GroupViewModel> GroupByOptions { get; }

        public ObservableList<SortViewModel> SortByOptions { get; }

        public void OnLoad(IGroupNameProvider groupNameProvider, ISortNameProvider sortNameProvider)
        {
            var groupVms = groupOptions.Select(x => new GroupViewModel
            {
                Group = x,
                Name = groupNameProvider.GroupByName(x)
            });

            var sortVms = sortOptions.Select(x => new SortViewModel
            {
                Sort = x,
                Name = sortNameProvider.SortByName(x)
            });

            GroupByOptions.AddRange(groupVms);
            SortByOptions.AddRange(sortVms);
        }
    }

    public interface IGroupNameProvider
    {
        string GroupByName(IGroup group);
    }

    public interface ISortNameProvider
    {
        string SortByName(ISort sort);
    }

    public class SortViewModel
    {
        public ISort Sort { get; set; }

        public string Name { get; set; } = "TILT";
    }
}
