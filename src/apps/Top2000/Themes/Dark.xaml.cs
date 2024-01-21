
using Microsoft.Maui.Controls.Xaml;

namespace Chroomsoft.Top2000.Apps.Themes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dark : ResourceDictionary
    {
        public const string ThemeName = "Dark";

        public Dark()
        {
            InitializeComponent();
        }
    }
}
