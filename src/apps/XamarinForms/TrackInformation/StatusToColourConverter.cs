using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Features.TrackInformation;
using Xamarin.Forms;

namespace Chroomsoft.Top2000.Apps.TrackInformation
{
    public class StatusToColourConverter : ValueConverterBase<ListingStatus, Color>
    {
        private static readonly Color YellowColour = new Color(255, 192, 0);
        private static readonly Color RedColour = new Color(221, 48, 57);
        private static readonly Color GreenColour = new Color(112, 173, 71);
        private static readonly Color GreyColour = new Color(103, 103, 103);

        public override Color Convert(ListingStatus value) => value switch
        {
            ListingStatus.New => YellowColour,
            ListingStatus.Back => YellowColour,
            ListingStatus.Decreased => RedColour,
            ListingStatus.Increased => GreenColour,
            _ => GreyColour,
        };
    }
}
