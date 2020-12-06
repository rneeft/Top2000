using Chroomsoft.Top2000.Apps.Globalisation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Privacy : ContentPage
    {
        public Privacy()
        {
            InitializeComponent();

            var source = new HtmlWebViewSource
            {
                Html = Translator.Instance["PrivacyStatement"]
            };

            WebViewer.Source = source;
        }
    }
}
