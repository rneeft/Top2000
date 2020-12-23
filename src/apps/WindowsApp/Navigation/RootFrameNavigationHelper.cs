using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Chroomsoft.Top2000.WindowsApp.Navigation
{
    /// <summary>
    /// RootFrameNavigationHelper registers for standard mouse and keyboard
    /// shortcuts used to go back and forward. There should be only one
    /// RootFrameNavigationHelper per view, and it should be associated with the
    /// root frame.
    /// </summary>
    /// <example>
    /// To make use of RootFrameNavigationHelper, create an instance of the
    /// RootNavigationHelper such as in the constructor of your root page.
    /// <code>
    ///     public MyRootPage()
    ///     {
    ///         this.InitializeComponent();
    ///         this.rootNavigationHelper = new RootNavigationHelper(MyFrame);
    ///     }
    /// </code>
    /// </example>
    [Windows.Foundation.Metadata.WebHostHidden]
    public class RootFrameNavigationHelper
    {
        private readonly SystemNavigationManager systemNavigationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootNavigationHelper"/> class.
        /// </summary>
        /// <param name="rootFrame">A reference to the top-level frame.
        /// This reference allows for frame manipulation and to register navigation handlers.</param>
        public RootFrameNavigationHelper(Frame rootFrame, Microsoft.UI.Xaml.Controls.NavigationView currentNavView)
        {
            this.Frame = rootFrame;
            this.Frame.Navigated += (s, e) =>
            {
                // Update the Back button whenever a navigation occurs.
                UpdateBackButton();
            };
            this.CurrentNavView = currentNavView;

            // Handle keyboard and mouse navigation requests
            this.systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += SystemNavigationManager_BackRequested;

            // must register back requested on navview
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {
                CurrentNavView.BackRequested += NavView_BackRequested;
            }

            // Listen to the window directly so we will respond to hotkeys regardless
            // of which element has focus.
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated +=
                CoreDispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed +=
                this.CoreWindow_PointerPressed;
        }

        private Frame Frame { get; set; }

        private Microsoft.UI.Xaml.Controls.NavigationView CurrentNavView { get; set; }

        private void NavView_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        private bool TryGoBack()
        {
            // don't go back if the nav pane is overlayed
            if (this.CurrentNavView.IsPaneOpen && (this.CurrentNavView.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Compact || this.CurrentNavView.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal))
            {
                return false;
            }

            bool navigated = false;
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                navigated = true;
            }

            return navigated;
        }

        private bool TryGoForward()
        {
            bool navigated = false;
            if (this.Frame.CanGoForward)
            {
                this.Frame.GoForward();
                navigated = true;
            }
            return navigated;
        }

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = TryGoBack();
            }
        }

        private void UpdateBackButton()
        {
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {
                this.CurrentNavView.IsBackEnabled = Frame.CanGoBack;
            }
            else
            {
                systemNavigationManager.AppViewBackButtonVisibility = this.Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            }
        }

        /// <summary>
        /// Invoked on every keystroke, including system keys such as Alt key combinations.
        /// Used to detect keyboard navigation between pages even when the page itself
        /// doesn't have focus.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender,
            AcceleratorKeyEventArgs e)
        {
            var virtualKey = e.VirtualKey;

            // Only investigate further when Left, Right, or the dedicated Previous or Next keys
            // are pressed
            if ((e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown ||
                e.EventType == CoreAcceleratorKeyEventType.KeyDown) &&
                (virtualKey == VirtualKey.Left || virtualKey == VirtualKey.Right ||
                (int)virtualKey == 166 || (int)virtualKey == 167))
            {
                var coreWindow = Window.Current.CoreWindow;
                var downState = CoreVirtualKeyStates.Down;
                bool menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
                bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
                bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;
                bool noModifiers = !menuKey && !controlKey && !shiftKey;
                bool onlyAlt = menuKey && !controlKey && !shiftKey;

                if (((int)virtualKey == 166 && noModifiers) ||
                    (virtualKey == VirtualKey.Left && onlyAlt))
                {
                    // When the previous key or Alt+Left are pressed navigate back
                    e.Handled = TryGoBack();
                }
                else if (((int)virtualKey == 167 && noModifiers) ||
                    (virtualKey == VirtualKey.Right && onlyAlt))
                {
                    // When the next key or Alt+Right are pressed navigate forward
                    e.Handled = TryGoForward();
                }
            }
        }

        /// <summary>
        /// Invoked on every mouse click, touch screen tap, or equivalent interaction.
        /// Used to detect browser-style next and previous mouse button clicks
        /// to navigate between pages.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        private void CoreWindow_PointerPressed(CoreWindow sender,
            PointerEventArgs e)
        {
            var properties = e.CurrentPoint.Properties;

            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed)
                return;

            // If back or forward are pressed (but not both) navigate appropriately
            bool backPressed = properties.IsXButton1Pressed;
            bool forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                e.Handled = true;
                if (backPressed) this.TryGoBack();
                if (forwardPressed) this.TryGoForward();
            }
        }
    }
}
