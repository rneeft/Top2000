using Chroomsoft.Top2000.Apps.Globalisation;

using Microsoft.Maui.Controls.Xaml;

namespace Chroomsoft.Top2000.Apps.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThirdParty : ContentPage
    {
        public ThirdParty()
        {
            InitializeComponent();

            var source = new HtmlWebViewSource
            {
                Html = Translator.Instance["Credits"]
            };

            WebViewer.Source = source;
        }
    }
}
