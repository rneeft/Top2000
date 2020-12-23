#nullable enable

using Chroomsoft.Top2000.WindowsApp.Common;
using System;
using System.Linq;
using Windows.System;
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

        async private void OnFeedbackClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=f53ae16e-51a4-4bf2-af3a-c6d5922331c1"));
        }

        async private void OnFacebookClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.facebook.com/Top2000App/"));
        }

        async private void OnEmailClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("mailto:rick@chroomsoft.nl"));
        }

        async private void OnVisitTop2000SiteClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.top2000.nl/"));
        }
    }
}
