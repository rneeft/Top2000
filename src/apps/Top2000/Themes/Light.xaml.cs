
using Microsoft.Maui.Controls.Xaml;

namespace Chroomsoft.Top2000.Apps.Themes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Light : ResourceDictionary
    {
        public const string ThemeName = "Light";

        public Light()
        {
            InitializeComponent();
        }
    }
}
