namespace Chroomsoft.Top2000.Apps.NavigationShell
{
    public interface IMainShell
    {
        bool IsViewForWhenTop2000IsLive { get; }

        void SetTitles();
    }
}
