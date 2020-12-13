using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.XamarinForms;
using Chroomsoft.Top2000.Features.AllEditions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.YearSelector
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage, IModalPage
    {
        private int startYear;

        public View()
        {
            BindingContext = App.GetService<ViewModel>();

            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        public Func<Edition, Task>? OnSelection { get; set; }

        public Task DismissAsync() => Navigation.PopModalAsync(animated: false);

        public async Task ShowAsModalDialog(int year, Func<Edition, Task> onSelection)
        {
            startYear = year;
            await ViewModel.LoadAllEditionsAsync(year);
            this.OnSelection = onSelection;
        }

        async private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OnSelection is null || e.CurrentSelection.Count == 0) return;

            var selectedEdition = (Edition)e.CurrentSelection[0];

            if (selectedEdition.Year != startYear)
            {
                await DismissAsync();
                await OnSelection.Invoke(selectedEdition);
            }
        }
    }
}
