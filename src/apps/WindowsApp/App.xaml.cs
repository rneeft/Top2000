using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.WindowsApp.Common;
using Chroomsoft.Top2000.WindowsApp.Navigation;
using Chroomsoft.Top2000.WindowsApp.TrackInformation;
using Chroomsoft.Top2000.WindowsApp.YearOverview;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xamarin.Essentials;

namespace Chroomsoft.Top2000.WindowsApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private static IServiceProvider? serviceProvider;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

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

        public static T GetService<T>() => ServiceProvider.GetService<T>();

        public static void InitialiseDependencyInjectionFramework()
        {
            ServiceProvider = new AppHostBuilder()
               .CreateDefaultAppHostBuilder()
               .ConfigureServices(ConfigureServices)
               .ConfigureLogging(ConfigureLogging)
               .Build()
               .Services;
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services
                .AddClientDatabase(new DirectoryInfo(FileSystem.AppDataDirectory))
                .AddMediatR(typeof(AllEditionsRequest).Assembly)
                .AddSingleton<NavigationRootPage>()
                .AddSingleton<YearOverviewPage>()
                .AddSingleton<TrackInformationViewModel>()
                .AddTransient<YearOverviewViewModel>()
                ;
        }

        public static void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.AddConsole(o => o.DisableColors = true);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // this.DebugSettings.EnableFrameRateCounter = true;
            }
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.BindingFailed += DebugSettings_BindingFailed;
            }
#endif

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            await EnsureWindow(args);
        }

        private static void FixSqLiteIssue()
        {
            SQLitePCL.Batteries.Init();
            var rc = SQLitePCL.raw.sqlite3_win32_set_directory(
            1, // data
            Windows.Storage.ApplicationData.Current.LocalFolder.Path
            );

            rc = SQLitePCL.raw.sqlite3_win32_set_directory(
                2, // temp
                Windows.Storage.ApplicationData.Current.TemporaryFolder.Path
                );
        }

        private static async Task EnsureDatabaseIsCreatedAsync()
        {
            var databaseGen = App.ServiceProvider.GetService<IUpdateClientDatabase>();
            var top2000 = App.ServiceProvider.GetService<Top2000AssemblyDataSource>();

            await databaseGen.RunAsync(top2000);
        }

        private void DebugSettings_BindingFailed(object sender, BindingFailedEventArgs e)
        {
            Debugger.Break();
        }

        private async Task EnsureWindow(IActivatedEventArgs args)
        {
            InitialiseDependencyInjectionFramework();
            await EnsureDatabaseIsCreatedAsync();

            var rootFrame = GetRootFrame();
            Type targetPageType = typeof(YearOverviewPage);

            var targetPageArguments = string.Empty;

            if (args.Kind == ActivationKind.Launch)
            {
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    try
                    {
                        await SuspensionManager.RestoreAsync().ConfigureAwait(false);
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
                targetPageArguments = ((LaunchActivatedEventArgs)args).Arguments;
            }

            rootFrame.Navigate(targetPageType, targetPageArguments);
            ((Microsoft.UI.Xaml.Controls.NavigationViewItem)(((NavigationRootPage)(Window.Current.Content)).NavigationView.MenuItems[0])).IsSelected = true;

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private Frame GetRootFrame()
        {
            Frame rootFrame;
            if (!(Window.Current.Content is NavigationRootPage rootPage))
            {
                rootPage = serviceProvider.GetRequiredService<NavigationRootPage>();
                rootFrame = (Frame)rootPage.FindName("rootFrame");
                if (rootFrame == null)
                {
                    throw new Exception("Root frame not found");
                }

                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
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
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync().ConfigureAwait(false);
            deferral.Complete();
        }
    }
}
