using Chroomsoft.Top2000.Apps.XamarinForms;

namespace Chroomsoft.Top2000.Apps.Overview.Date
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage
    {
        public View()
        {
            BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        private static int FirstVisibleItemIndex { get; set; }

        public async Task ScrollToCorrectPositionAsync(int index, ScrollToPosition scrollToPosition = ScrollToPosition.Start)
        {
            var tries = 0;

            while (index != FirstVisibleItemIndex && tries < 6)
            {
                listings.ScrollTo(index, position: scrollToPosition, animate: false);

                tries++;

                await Task.Delay(200);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.GroupFlyout.IsVisible)
            {
                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                this.GroupFlyout.TranslateTo(this.Width * -1, 0);
                this.GroupFlyout.IsVisible = false;

                return true;
            }

            if (this.trackInformation.IsVisible)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CloseTrackInformationAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return true;
            }

            return base.OnBackButtonPressed();
        }
    }
}