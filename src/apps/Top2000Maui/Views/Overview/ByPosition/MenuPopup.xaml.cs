using CommunityToolkit.Maui.Views;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;

public partial class MenuPopup : Popup
{
    public MenuPopup()
    {
        InitializeComponent();
    }

    private void OnOKButtonClicked(object sender, EventArgs e)
    {
        Close();
    }
}