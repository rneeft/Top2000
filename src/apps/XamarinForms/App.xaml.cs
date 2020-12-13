using Chroomsoft.Top2000.Data.ClientDatabase;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chroomsoft.Top2000.Apps.XamarinForms
{
    public class FactoryFor<TView> : RouteFactorFor<TView> where TView : notnull
    {
        public override Element GetOrCreate() => (App.GetService<TView>() as Element)
            ?? throw new InvalidOperationException("TView must of Element type");
    }

    public abstract class RouteFactorFor<TView> : RouteFactory where TView : notnull
    { }

    public partial class App : Application
    {
        private static IServiceProvider? serviceProvider;

        public App()
        {
            InitializeComponent();

            MainPage = GetService<NavigationShell.View>();
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

        //private static async Task CheckForOnlineUpdates()
        //{
        //    await Task.Delay(5 * 1000);

        //    var checker = App.GetService<IOnlineUpdateChecker>();
        //    await checker.UpdateAsync();
        //}
    }
}
