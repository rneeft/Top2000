using Chroomsoft.Top2000.Data;
using SQLite;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WindowsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Top2000Data top2000data;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async Task<(int index, string name, string contents)> GetSqlFileAsync(string fileName)
        {
            var contents = await top2000data.GetScriptContentAsync(fileName);
            var splittedName = fileName.Split('-');
            var index = int.Parse(splittedName[0].Trim(), CultureInfo.InvariantCulture.NumberFormat);
            var name = splittedName[1].Trim();

            return (index, name, contents);
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            top2000data = new Top2000Data();
            var sqls = await Task.WhenAll
            (
                top2000data
                    .GetAllSqlFiles()
                    .Select(GetSqlFileAsync)
            );

            var currentDir = Directory.GetCurrentDirectory();
            var connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "top2000data.db");

            if (File.Exists(connectionString))
                File.Delete(connectionString);

            var connection = new SQLiteAsyncConnection("top2000data.db", SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            var readOnlyConnection = new SQLiteAsyncConnection("top2000data.db", SQLiteOpenFlags.ReadOnly);

            foreach (var file in sqls)
            {
                await connection.RunInTransactionAsync(x =>
                {
                    string line;
                    try
                    {
                        var lines = file.contents.Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .ToList();

                        foreach (var l in lines)
                        {
                            line = l;
                            x.Execute(line);
                        }
                        // x.Insert(new VersionJournal { Version = item.Version, Title = item.Name });
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                });
            }
        }
    }
}
