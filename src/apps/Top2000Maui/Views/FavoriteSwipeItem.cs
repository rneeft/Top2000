namespace Chroomsoft.Top2000.Apps.Views;

internal sealed class FavoriteSwipeItem : SwipeItem
{
    public static readonly BindableProperty IsFavoriteProperty =
      BindableProperty.Create(nameof(IsFavorite), typeof(bool), typeof(FavoriteSwipeItem), false, BindingMode.OneWay, propertyChanged: OnIsFavoriteChanged);

    public bool IsFavorite
    {
        get => (bool)GetValue(IsFavoriteProperty);
        set => SetValue(IsFavoriteProperty, value);
    }

    private static void OnIsFavoriteChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var item = (FavoriteSwipeItem)bindable;

        item.RefreshIcon();
    }

    private void RefreshIcon()
    {
        var fontImageSource = (FontImageSource)this.IconImageSource;
        fontImageSource.Glyph = IsFavorite
            ? Symbols.NonFavorite
            : Symbols.Favorite;
    }
}