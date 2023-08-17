namespace Chroomsoft.Top2000.Apps;

public partial class App : Application
{
    public static readonly IFormatProvider DateTimeFormatProvider = DateTimeFormatInfo.InvariantInfo;

    public static readonly IFormatProvider NumberFormatProvider = NumberFormatInfo.InvariantInfo;

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}