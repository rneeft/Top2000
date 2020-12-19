using Chroomsoft.Top2000.Apps.XamarinForms;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        public Task LoadTrackDetailsAsync(int trackId, Func<Task> onClose)
        {
            this.OnClose = onClose;
            positions.ScrollTo(0);

            return ViewModel.LoadTrackDetailsAsync(trackId);
        }

        async private void OnCloseButtonClick(object sender, System.EventArgs e)
        {
            if (OnClose != null)
            {
                await OnClose.Invoke();
            }
        }

        async private void OnViewVideoClick(object sender, EventArgs e)
        {
            var trackTitle = ViewModel.Title.Replace(' ', '+');
            var artistName = ViewModel.Artist.Replace(' ', '+');

            var url = new Uri($"https://duckduckgo.com/?q=!ducky+onsite:www.youtube.com+{trackTitle}+{artistName}");

            await Launcher.OpenAsync(url);
        }
    }
}
