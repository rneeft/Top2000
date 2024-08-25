using Chroomsoft.Top2000.Apps.Common;
using System;
using System.Globalization;

namespace Chroomsoft.Top2000.Apps.Overview
{
    public class EditionPlayTimeConverter : ValueConverterBase<DateTime, string>
    {
        private const string ShortFormat = "dd MMM yyyy HH:mm";
        private static readonly IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;

        public override string Convert(DateTime dateTime) => dateTime.ToString(ShortFormat, formatProvider);
    }
}
