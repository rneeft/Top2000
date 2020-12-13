using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.XamarinForms;
using Chroomsoft.Top2000.Features.AllEditions;
using MediatR;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.NavigationShell
{
    public class SplitConverter : ValueConverterBase<string, string, string>
    {
        public override string Convert(string value, string param)
        {
            return value.Split(';')[int.Parse(param)];
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : Shell, INavigationOptions
    {
        private const string ShortFormat = "dd MMM yyyy HH:mm";

        private static readonly IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;

        public View()
        {
            this.BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public bool IsDatesVisible
        {
            get { return DateContent.IsVisible; }
            set { DateContent.IsVisible = value; }
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        async protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            await ViewModel.LoadAllEditionsAsync();
        }

        private void OnMenuItemClicked(object sender, System.EventArgs e)
        {
            var stack = ((StackLayout)sender);
            var previousbgc = stack.BackgroundColor;
            ((StackLayout)sender).BackgroundColor = Color.Red;
            Shell.Current.FlyoutIsPresented = false;

            ((StackLayout)sender).BackgroundColor = previousbgc;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }

    public class EditionPlayTimeConverter : ValueConverterBase<Edition, string>
    {
        private const string ShortFormat = "dd MMM yyyy HH:mm";
        private static readonly IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;

        public override string Convert(Edition value)
        => $"{value.LocalStartDateAndTime.ToString(ShortFormat, formatProvider)} - {value.LocalEndDateAndTime.ToString(ShortFormat, formatProvider)}";
    }

    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Editions = new ObservableList<Edition>();
        }

        public ObservableList<Edition> Editions { get; }

        public Edition? SelectedEdition
        {
            get { return GetPropertyValue<Edition?>(); }
            set { SetPropertyValue(value); }
        }

        public async Task LoadAllEditionsAsync()
        {
            Editions.ClearAddRange(await mediator.Send(new AllEditionsRequest()));
            //  TrySelectingEdition(year);
        }

        private void TrySelectingEdition(int year)
        {
            SelectedEdition = Editions.SingleOrDefault(x => x.Year == year);
        }
    }

    public interface INavigationOptions
    {
        bool IsDatesVisible { get; set; }
    }
}
