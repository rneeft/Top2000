#nullable enable

using Chroomsoft.Top2000.WindowsApp.Common;
using Microsoft.AppCenter.Analytics;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using winui = Microsoft.UI.Xaml.Controls;

namespace Chroomsoft.Top2000.WindowsApp.Navigation
{
    public sealed partial class View : Page
    {
        public View()
        {
            var globalNotifier = App.GetService<IGlobalUpdate>();
            globalNotifier.NotificationHandler = ShowNotification;

            this.InitializeComponent();

            Window.Current.SetTitleBar(AppTitleBar);

            CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle(s);

            // remove the solid-colored backgrounds behind the caption controls and system back button if we are in left mode
            // This is done when the app is loaded since before that the actual theme that is used is not "determined" yet
            Loaded += delegate (object sender, RoutedEventArgs e)
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            };
        }

        public Microsoft.UI.Xaml.Controls.NavigationView NavigationView
        {
            get { return NavigationViewControl; }
        }

        public void ShowNotification()
        {
            UpdateNotification.Show();
        }

        private void UpdateAppTitle(CoreApplicationViewTitleBar coreTitleBar)
        {
            //ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        private void OnNavigationViewItemInvoked(winui.NavigationView _, winui.NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.IsSelected)
            {
                // Clicked on an item that is already selected,
                // Avoid navigating to the same page again causing movement.
                return;
            }

            if (args.IsSettingsInvoked)
            {
                Analytics.TrackEvent("SettingsOpen");
                if (rootFrame.CurrentSourcePageType != typeof(About.View))
                {
                    rootFrame.Navigate(typeof(About.View));
                }
            }
            else
            {
                var invokedItem = args.InvokedItemContainer;

                if (invokedItem == OverviewItem)
                {
                    rootFrame.Navigate(typeof(YearOverview.View));
                }

                if (invokedItem == SearchItem)
                {
                    rootFrame.Navigate(typeof(Searching.View));
                }
            }
        }

        private void rootFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            rootFrame.Navigated -= rootFrame_Navigated;

            var startupTime = App.GetStartupTime();

            if (startupTime != null)
            {
                Analytics.TrackEvent("Startup", new Dictionary<string, string>
                {
                    { "StartTotalSeconds", "" + startupTime.Value.TotalSeconds },
                    { "StartTotalMilliseconds", "" + startupTime.Value.TotalMilliseconds }
                });
            }
        }
    }
}
