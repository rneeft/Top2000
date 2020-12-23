using Android.App;
using Android.Content.PM;
using Android.OS;

namespace XamarinForms.Droid
{
    [Activity(Label = "Top 2000", RoundIcon = "@mipmap/top2000icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            LoadApplication(SplashActivity.App);
        }
    }
}
