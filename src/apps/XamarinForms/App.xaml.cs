using Chroomsoft.Top2000.Apps.Globalisation;
using Chroomsoft.Top2000.Apps.Themes;
using Chroomsoft.Top2000.Data.ClientDatabase;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chroomsoft.Top2000.Apps.XamarinForms
{
    public partial class App : Application
    {
        public static readonly IFormatProvider DateTimeFormatProvider = DateTimeFormatInfo.InvariantInfo;

        public static readonly IFormatProvider NumberFormatProvider = NumberFormatInfo.InvariantInfo;

        private static IServiceProvider? serviceProvider;

        public App()
        {
            InitializeComponent();
            SetCulture();
            SetTheme();

            var navigationShell = GetService<NavigationShell.View>();
            MainPage = navigationShell;

            navigationShell.SetTitles();
        }

        public static IServiceProvider ServiceProvider
        {
            get
            {
                return serviceProvider ??
                    throw new InvalidOperationException("Application isn't booted yet");
            }
            set
            {
                serviceProvider = value;
            }
        }

        public static T GetService<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();

        public static Task EnsureDatabaseIsCreatedAsync()
        {
            var databaseGen = GetService<IUpdateClientDatabase>();
            var top2000 = GetService<Top2000AssemblyDataSource>();

            return databaseGen.RunAsync(top2000);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void SetCulture()
        {
            var localisationService = App.GetService<ILocalisationService>();
            localisationService.SetCultureFromSetting();
        }

        private void SetTheme()
        {
            var themeService = GetService<IThemeService>();
            themeService.SetThemeFromSetting();
        }

        //private static async Task CheckForOnlineUpdates()
        //{
        //    await Task.Delay(5 * 1000);

        //    var checker = App.GetService<IOnlineUpdateChecker>();
        //    await checker.UpdateAsync();
        //}
    }
}
