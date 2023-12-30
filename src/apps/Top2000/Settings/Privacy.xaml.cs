using Chroomsoft.Top2000.Apps.Globalisation;

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

        protected override bool OnBackButtonPressed()
        {
            if (this.WebViewer.CanGoBack)
            {
                this.WebViewer.GoBack();
                return true;
            }

            return base.OnBackButtonPressed();
        }
    }
}
