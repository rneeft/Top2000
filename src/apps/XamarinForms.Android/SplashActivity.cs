#nullable disable

using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Chroomsoft.Top2000.Apps;
using Chroomsoft.Top2000.Apps.AskForReview;
using Chroomsoft.Top2000.Apps.Globalisation;
using Chroomsoft.Top2000.Apps.XamarinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XamarinForms.Droid.AskForReview;
using XamarinForms.Droid.Globalisation;

namespace XamarinForms.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash",
                  Icon = "@mipmap/top2000icon",
                  RoundIcon = "@mipmap/top2000iconRound",
                  MainLauncher = true,
                  NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        public static App App { get; set; }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            App.ServiceProvider = AppHostBuilder.CreateServices(PlatformServices);
            App.EnsureDatabaseIsCreatedAsync().GetAwaiter().GetResult();

            Xamarin.Forms.Forms.SetFlags("Brush_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            App = new App();

            StartActivity(typeof(MainActivity));
            Finish();

#pragma warning disable CS4014 // Don't wait for this online update
            App.CheckForOnlineUpdates();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void PlatformServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddSingleton<ILocalisationService, LocalisationService>();
            services.AddTransient<IStoreReview, StoreReviewImplementation>();
        }
    }
}
