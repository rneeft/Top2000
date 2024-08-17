using System;
using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.XamarinForms;

namespace Chroomsoft.Top2000.Apps.Overview
{
    public class DeltaToStringConverter : ValueConverterBase<int?, string>
    {
        public override string Convert(int? offset)
        {
            return offset.HasValue && offset.Value != 0
                ? Math.Abs(offset.Value).ToString(App.NumberFormatProvider)
                : string.Empty;
        }
    }
}
