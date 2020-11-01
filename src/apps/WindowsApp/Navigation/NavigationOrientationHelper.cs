using Windows.UI.ViewManagement;

namespace Chroomsoft.Top2000.WindowsApp.Navigation
{
    public static class NavigationOrientationHelper
    {
        public static void UpdateTitleBar()
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            var userSettings = new UISettings();
            titleBar.ButtonBackgroundColor = userSettings.GetColorValue(UIColorType.Accent);
            titleBar.ButtonInactiveBackgroundColor = userSettings.GetColorValue(UIColorType.Accent);
        }
    }
}
