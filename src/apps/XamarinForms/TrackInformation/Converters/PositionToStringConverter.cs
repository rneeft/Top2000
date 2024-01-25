using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.XamarinForms;

namespace Chroomsoft.Top2000.Apps.TrackInformation
{
    public class PositionToStringConverter : ValueConverterBase<int?, string>
    {
        public override string Convert(int? value)
        {
            return value.HasValue
                ? value.Value.ToString(App.NumberFormatProvider)
                : "-";
        }
    }
}
