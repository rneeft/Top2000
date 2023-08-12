using Chroomsoft.Top2000.Data.ClientDatabase.Sources;

namespace Chroomsoft.Top2000.Apps
{
    public partial class AppShell : Shell
    {
        private readonly IUpdateClientDatabase updateClientDatabase;
        private readonly Top2000AssemblyDataSource top2000AssemblyDataSource;

        public AppShell(IUpdateClientDatabase updateClientDatabase, Top2000AssemblyDataSource top2000AssemblyDataSource)
        {
            InitializeComponent();
            this.updateClientDatabase = updateClientDatabase;
            this.top2000AssemblyDataSource = top2000AssemblyDataSource;
        }

        private async void Shell_Loaded(object sender, EventArgs e)
        {
            await this.updateClientDatabase.RunAsync(this.top2000AssemblyDataSource);
        }
    }
}