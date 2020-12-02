#nullable enable

using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.WindowsApp.Common;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp
{
    sealed partial class App : Application
    {
        private static IServiceProvider? serviceProvider;
        private static Stopwatch? startupWatch = Stopwatch.StartNew();

        public App()
        {
            AppCenter.Start("a73816a5-fcfd-4cdf-9a34-8413c2f22190",
                   typeof(Analytics), typeof(Crashes));

            this.InitializeComponent();

            FixSqLiteIssue();
        }

        public static IServiceProvider ServiceProvider
        {
            get
            {
                return serviceProvider ?? throw new InvalidOperationException("Application isn't booted yet");
            }
            set
            {
                serviceProvider = value;
            }
        }

        public static TimeSpan? GetStartupTime()
        {
            if (startupWatch is null) return null;

            var time = startupWatch.Elapsed;

            startupWatch.Stop();
            startupWatch = null;

            return time;
        }

        public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }

        public static T GetService<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            ServiceProvider = AppHostBuilder.CreateServices();

            await EnsureDatabaseIsCreatedAsync();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CheckForOnlineUpdates();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            EnsureWindow(args);
        }

        private static void FixSqLiteIssue()
        {
            SQLitePCL.Batteries.Init();
            SQLitePCL.raw.sqlite3_win32_set_directory(1, ApplicationData.Current.LocalFolder.Path);
            SQLitePCL.raw.sqlite3_win32_set_directory(2, ApplicationData.Current.TemporaryFolder.Path);
        }

        private static Task EnsureDatabaseIsCreatedAsync()
        {
            var databaseGen = GetService<IUpdateClientDatabase>();
            var top2000 = GetService<Top2000AssemblyDataSource>();

            return databaseGen.RunAsync(top2000);
        }

        private static async Task CheckForOnlineUpdates()
        {
            await Task.Delay(5 * 1000);

            var checker = App.GetService<IOnlineUpdateChecker>();
            await checker.UpdateAsync();
        }

        private void EnsureWindow(IActivatedEventArgs args)
        {
            var rootFrame = GetRootFrame();

            ThemeHelper.Initialize();

            var targetPageArguments = string.Empty;

            if (args.Kind == ActivationKind.Launch)
            {
                targetPageArguments = ((LaunchActivatedEventArgs)args).Arguments;
            }

            rootFrame.Navigate(typeof(YearOverview.View), targetPageArguments);
            ((Microsoft.UI.Xaml.Controls.NavigationViewItem)(((Navigation.View)(Window.Current.Content)).NavigationView.MenuItems[0])).IsSelected = true;

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private Frame GetRootFrame()
        {
            Frame rootFrame;
            if (!(Window.Current.Content is Navigation.View rootPage))
            {
                rootPage = ServiceProvider.GetRequiredService<Navigation.View>();
                rootFrame = (Frame)rootPage.FindName("rootFrame")
                    ?? throw new Exception("Root frame not found");

                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootPage;
            }
            else
            {
                rootFrame = (Frame)rootPage.FindName("rootFrame");
            }

            return rootFrame;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new InvalidOperationException("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
