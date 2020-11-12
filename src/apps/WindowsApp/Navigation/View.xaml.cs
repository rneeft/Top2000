#nullable enable

using Chroomsoft.Top2000.Features.Searching;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using winui = Microsoft.UI.Xaml.Controls;

namespace Chroomsoft.Top2000.WindowsApp.Navigation
{
    public sealed partial class View : Page
    {
        public static View Current;
        public static Frame RootFrame = null;
        private RootFrameNavigationHelper _navHelper;

        public View()
        {
            ViewModel = App.GetService<ViewModel>();
            this.InitializeComponent();

            // Workaround for VisualState issue that should be fixed
            // by https://github.com/microsoft/microsoft-ui-xaml/pull/2271

            _navHelper = new RootFrameNavigationHelper(rootFrame, NavigationViewControl);

            Current = this;
            RootFrame = rootFrame;

            this.GotFocus += (object sender, RoutedEventArgs e) =>
            {
                // helpful for debugging focus problems w/ keyboard & gamepad
                if (FocusManager.GetFocusedElement() is FrameworkElement focus)
                {
                    Debug.WriteLine("got focus: " + focus.Name + " (" + focus.GetType().ToString() + ")");
                }
            };

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

        public ViewModel ViewModel { get; }

        private void UpdateAppTitle(CoreApplicationViewTitleBar coreTitleBar)
        {
            //ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
        {
            var c = e.Content;
        }

        //private void NavigationViewControl_PaneOpened(winui.NavigationView sender, object args)
        //{
        //    UpdateAppTitleMargin(sender);
        //}

        //private void NavigationViewControl_PaneClosing(winui.NavigationView sender, winui.NavigationViewPaneClosingEventArgs args)
        //{
        //    UpdateAppTitleMargin(sender);
        //}

        //private void UpdateHeaderMargin(Microsoft.UI.Xaml.Controls.NavigationView sender)
        //{
        //    if (PageHeader != null)
        //    {
        //        if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
        //        {
        //            Current.PageHeader.HeaderPadding = (Thickness)App.Current.Resources["PageHeaderMinimalPadding"];
        //        }
        //        else
        //        {
        //            Current.PageHeader.HeaderPadding = (Thickness)App.Current.Resources["PageHeaderDefaultPadding"];
        //        }
        //    }
        //}

        //public PageHeader PageHeader
        //{
        //    get
        //    {
        //        return UIHelper.GetDescendantsOfType<PageHeader>(NavigationViewControl).FirstOrDefault();
        //    }
        //}

        //private void NavigationViewControl_DisplayModeChanged(winui.NavigationView sender, winui.NavigationViewDisplayModeChangedEventArgs args)
        //{
        //    Thickness currMargin = AppTitleBar.Margin;
        //    if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
        //    {
        //        AppTitleBar.Margin = new Thickness((sender.CompactPaneLength * 2), currMargin.Top, currMargin.Right, currMargin.Bottom);
        //    }
        //    else
        //    {
        //        AppTitleBar.Margin = new Thickness(sender.CompactPaneLength, currMargin.Top, currMargin.Right, currMargin.Bottom);
        //    }
        //    UpdateAppTitleMargin(sender);
        //}

        //private void UpdateAppTitleMargin(Microsoft.UI.Xaml.Controls.NavigationView sender)
        //{
        //    const int smallLeftIndent = 4, largeLeftIndent = 24;

        //    if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
        //    {
        //        AppTitle.TranslationTransition = new Vector3Transition();

        //        if ((sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
        //                 sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
        //        {
        //            AppTitle.Translation = new System.Numerics.Vector3(smallLeftIndent, 0, 0);
        //        }
        //        else
        //        {
        //            AppTitle.Translation = new System.Numerics.Vector3(largeLeftIndent, 0, 0);
        //        }
        //    }
        //    else
        //    {
        //        Thickness currMargin = AppTitle.Margin;

        //        if ((sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
        //                 sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
        //        {
        //            AppTitle.Margin = new Thickness(smallLeftIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
        //        }
        //        else
        //        {
        //            AppTitle.Margin = new Thickness(largeLeftIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
        //        }
        //    }
        //}

        private void OnNavigationViewItemInvoked(winui.NavigationView sender, winui.NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.IsSelected)
            {
                // Clicked on an item that is already selected,
                // Avoid navigating to the same page again causing movement.
                return;
            }

            if (args.IsSettingsInvoked)
            {
                //if (rootFrame.CurrentSourcePageType != typeof(SettingsPage))
                //{
                //    rootFrame.Navigate(typeof(SettingsPage));
                //}
            }
            else
            {
                //var invokedItem = args.InvokedItemContainer;
                //if (invokedItem == _allControlsMenuItem)
                //{
                //    if (rootFrame.CurrentSourcePageType != typeof(AllControlsPage))
                //    {
                //        rootFrame.Navigate(typeof(AllControlsPage));
                //    }
                //}
                //else if (invokedItem == _newControlsMenuItem)
                //{
                //    if (rootFrame.CurrentSourcePageType != typeof(NewControlsPage))
                //    {
                //        rootFrame.Navigate(typeof(NewControlsPage));
                //    }
                //}
                //else
                //{
                //    if (invokedItem.DataContext is ControlInfoDataGroup)
                //    {
                //        var itemId = ((ControlInfoDataGroup)invokedItem.DataContext).UniqueId;
                //        rootFrame.Navigate(typeof(SectionPage), itemId);
                //    }
                //    else if (invokedItem.DataContext is ControlInfoDataItem)
                //    {
                //        var item = (ControlInfoDataItem)invokedItem.DataContext;
                //        rootFrame.Navigate(typeof(ItemPage), item.UniqueId);
                //    }
                //}
            }
        }

        private void rootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
        }

        private void OnSearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Frame.Navigate(typeof(Searching.View), new SearchNavigationData
            {
                QueryText = ViewModel.SeachText,
                TrackId = ViewModel.SelectedSuggestion?.Id
            });
        }

        private void OnSearchSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ViewModel.SelectedSuggestion = (Track)args.SelectedItem;
        }
    }

    public class SearchNavigationData
    {
        public string QueryText { get; set; } = string.Empty;

        public int? TrackId { get; set; }
    }

    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Suggestions = new ObservableList<Track>();
        }

        public ObservableList<Track> Suggestions { get; }

        public Track? SelectedSuggestion
        {
            get { return GetPropertyValue<Track?>(); }
            set { SetPropertyValue(value); }
        }

        public string SeachText
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public async Task SearchAsync()
        {
            var request = new SearchTrackRequest(SeachText, new SortByTitle(), new GroupByNothing());
            var result = await mediator.Send(request);
            Suggestions.AddRange(result.SelectMany(x => x));
        }
    }
}
