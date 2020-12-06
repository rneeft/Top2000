using Chroomsoft.Top2000.Apps.XamarinForms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.Overview
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage
    {
        public View()
        {
            InitializeComponent();
        }

        private void ShowDatesView(object sender, System.EventArgs e)
        {
            var view = App.GetService<NavigationShell.View>();
            view.ShowDatesView();
        }

        private void HideDatesView(object sender, System.EventArgs e)
        {
            var view = App.GetService<NavigationShell.View>();
            view.HideDatesView();
        }

        async private void Button_Clicked(object sender, System.EventArgs e)
        {
            var page = App.GetService<YearSelector.View>();
            await Navigation.PushModalAsync(new NavigationPage(page));
        }
    }
}
