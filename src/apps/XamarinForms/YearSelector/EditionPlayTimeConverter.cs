using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Features.AllEditions;
using System;
using System.Globalization;

namespace Chroomsoft.Top2000.Apps.YearSelector
{
    public class EditionPlayTimeConverter : ValueConverterBase<Edition, string>
    {
        private const string ShortFormat = "dd MMM yyyy HH:mm";
        private static readonly IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;

        public override string Convert(Edition value)
        => $"{value.LocalStartDateAndTime.ToString(ShortFormat, formatProvider)} - {value.LocalEndDateAndTime.ToString(ShortFormat, formatProvider)}";
    }
}
