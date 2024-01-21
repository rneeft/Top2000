using Android.Content;
using AndroidX.Core.Content;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Chroomsoft.Top2000.Apps.Platforms.Android;

public class MyShellRenderer : ShellRenderer
{
    public MyShellRenderer(Context context) : base(context)
    {
    }

    protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
    {
        return new MyToolbarAppearanceTracker();
    }
}

internal class MyToolbarAppearanceTracker : IShellToolbarAppearanceTracker
{
    public void Dispose()
    {
    }

    public void ResetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker)
    {
    }

    public void SetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
    {
        if (toolbar is not null && toolbar.OverflowIcon is not null && Platform.CurrentActivity is not null)
        {
            toolbar.OverflowIcon.SetTint(ContextCompat.GetColor(Platform.CurrentActivity, Resource.Color.white));
        }
    }
}