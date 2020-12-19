using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.XamarinForms;

namespace Chroomsoft.Top2000.Apps.TrackInformation
{
    public class OffsetToStringConverter : ValueConverterBase<int?, string>
    {
        public override string Convert(int? offset)
        {
            return offset.HasValue && offset.Value != 0
                ? PositiveInteger(offset.Value).ToString(App.NumberFormatProvider)
                : string.Empty;
        }

        private int PositiveInteger(int value)
        {
            return value < 0
                ? value * -1
                : value;
        }
    }
}
