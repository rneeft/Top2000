namespace Chroomsoft.Top2000.Apps.Views.TrackInformation;

internal sealed class FavoriteToolbarItem : ToolbarItem
{
    public static readonly BindableProperty IsFavoriteProperty =
       BindableProperty.Create(nameof(IsFavorite), typeof(bool), typeof(FavoriteToolbarItem), true, BindingMode.OneWay, propertyChanged: OnIsFavoriteChanged);

    public bool IsFavorite
    {
        get => (bool)GetValue(IsFavoriteProperty);
        set => SetValue(IsFavoriteProperty, value);
    }

    private static void OnIsFavoriteChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var item = (FavoriteToolbarItem)bindable;

        item.RefreshVisibility();
    }

    private void RefreshVisibility()
    {
        var fontImageSource = (FontImageSource)this.IconImageSource;
        fontImageSource.Glyph = IsFavorite
            ? Symbols.Favorite
            : Symbols.NonFavorite;
    }
}