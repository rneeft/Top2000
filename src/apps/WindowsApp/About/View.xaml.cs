#nullable enable

using Chroomsoft.Top2000.WindowsApp.Common;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.About
{
    public sealed partial class View : Page
    {
        public View()
        {
            ViewModel = App.GetService<ViewModel>();
            this.InitializeComponent();
        }

        public ViewModel ViewModel { get; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CreditsWebView.NavigateToString(CreditsText.Text);
            PrivacyWebView.NavigateToString(PrivacyText.Text);

            await ViewModel.LoadViewModelAsync();

            var currentTheme = ThemeHelper.RootTheme.ToString();
            (ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme)).IsChecked = true;
        }

        private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
            if (selectedTheme != null)
                ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(selectedTheme);
        }
    }
}
