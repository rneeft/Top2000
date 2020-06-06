using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Windows.UI.Xaml.Controls;
using WindowsApp.DatabaseService;

namespace WindowsApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        async private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var databaseGen = App.ServiceProvider.GetService<ICreateAndUpgradeDatabase>();
            await databaseGen.GenerateOrUpdateAsync();
        }

        private void LoadClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var text = File.ReadAllText(path.Text, System.Text.Encoding.UTF8);
            contents.Text = text;
        }

        private void SaveClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            File.WriteAllText(path.Text, contents.Text, System.Text.Encoding.UTF8);
        }
    }
}
