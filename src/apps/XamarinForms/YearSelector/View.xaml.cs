using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.XamarinForms;
using Chroomsoft.Top2000.Features.AllEditions;
using MediatR;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.YearSelector
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage, IModalPage
    {
        public View()
        {
            BindingContext = App.GetService<ViewModel>();

            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        public Task DismissAsync() => Navigation.PopModalAsync();

        async protected override void OnAppearing()
        {
            await ViewModel.LoadAllEditionsAsync();
        }
    }

    public class ViewModel
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Editions = new ObservableList<Edition>();
        }

        public ObservableList<Edition> Editions { get; }

        public async Task LoadAllEditionsAsync()
        {
            if (Editions.Count == 0)
            {
                Editions.ClearAddRange(await mediator.Send(new AllEditionsRequest()));
            }
        }
    }
}
