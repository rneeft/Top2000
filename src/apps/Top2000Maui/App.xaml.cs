namespace Chroomsoft.Top2000.Apps;

public partial class App : Application
{
    public static readonly IFormatProvider DateTimeFormatProvider = DateTimeFormatInfo.InvariantInfo;

    public static readonly IFormatProvider NumberFormatProvider = NumberFormatInfo.InvariantInfo;

    public App()
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjY0Nzg1N0AzMjMyMmUzMDJlMzBtRXNLaExiZGhMczArTlppemV1bXZwOXFaa3FZdHgzMERsaElCWU9wOEE0PQ==");

        InitializeComponent();

        MainPage = new AppShell();
    }
}