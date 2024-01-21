﻿namespace Chroomsoft.Top2000.Apps.Views.Overview;

public class TrackListingSymbolFontSizeConverter : ValueConverterBase<int?, double>
{
    public override double Convert(int? value)
    {
        if (value is null || !value.HasValue || value.Value == 0)
        {
            return 20.0;
        }

        return 11.0;
    }
}

public class BoolToSymbolConverter : ValueConverterBase<bool, string>
{
    public override string Convert(bool value)
    {
        return value ? Symbols.Favorite : Symbols.NonFavorite;
    }
}