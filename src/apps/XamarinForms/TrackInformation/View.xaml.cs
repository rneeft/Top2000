using Chroomsoft.Top2000.Apps.XamarinForms;

namespace Chroomsoft.Top2000.Apps.TrackInformation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : Grid
    {
        public View()
        {
            BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        private Func<Task>? OnClose { get; set; }

        public async Task LoadTrackDetailsAsync(int trackId, Func<Task> onClose)
        {
            this.OnClose = onClose;
            await positionsScroll.ScrollToAsync(0, 0, animated: false);

            await ViewModel.LoadTrackDetailsAsync(trackId);
        }

        private async void OnCloseButtonClick(object sender, System.EventArgs e)
        {
            if (OnClose != null)
            {
                await OnClose.Invoke();
            }
        }
    }
}