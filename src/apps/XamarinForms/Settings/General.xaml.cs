using Chroomsoft.Top2000.Apps.Globalisation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class General : ContentPage
    {
        private string? darkThemeText;
        private string? lightThemeText;

        public General()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            darkThemeText = Translator.Instance["DarkTheme"];
            lightThemeText = Translator.Instance["LightTheme"];

            ThemePicker.Items.Add(darkThemeText);
            ThemePicker.Items.Add(lightThemeText);

            
        }

        private void OnThemeChanged(object sender, System.EventArgs e)
        {
            if (darkThemeText is null || lightThemeText is null) return;

            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (ThemePicker.SelectedIndex)
                {
                    case 0:
                        mergedDictionaries.Add(new Themes.Dark());
                        break;

                    default:
                        mergedDictionaries.Add(new Themes.Light());
                        break;
                }
            }
        }
    }
}
