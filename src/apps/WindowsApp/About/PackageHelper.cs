#nullable enable

using Windows.ApplicationModel;

namespace Chroomsoft.Top2000.WindowsApp.About
{
    public static class PackageHelper
    {
        public static string GetAppVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        public static void RequestFeedback()
        {
            // not implemented yet, don't bug use with this yet.
        }
    }
}
