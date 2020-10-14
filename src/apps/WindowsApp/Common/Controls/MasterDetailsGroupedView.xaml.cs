using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;

namespace Chroomsoft.Top2000.WindowsApp.Controls
{
    public class MasterDetailsGroupedView : MasterDetailsView
    {
        public static readonly DependencyProperty GroupedItemTemplateProperty = DependencyProperty.Register(
            nameof(GroupedItemTemplate),
            typeof(DataTemplate),
            typeof(MasterDetailsGroupedView),
            new PropertyMetadata(null));

        public DataTemplate GroupedItemTemplate
        {
            get { return (DataTemplate)GetValue(GroupedItemTemplateProperty); }
            set { SetValue(GroupedItemTemplateProperty, value); }
        }
    }
}
