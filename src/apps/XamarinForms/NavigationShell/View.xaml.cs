using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.NavigationShell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : Shell, INavigationOptions
    {
        public View()
        {
            InitializeComponent();
        }

        public void HideDatesView()
        {
            DateContent.IsVisible = false;
        }

        public void ShowDatesView()
        {
            DateContent.IsVisible = true;
        }
    }

    public interface INavigationOptions
    {
        void ShowDatesView();

        void HideDatesView();
    }
}
