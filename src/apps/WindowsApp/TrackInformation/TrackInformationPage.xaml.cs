using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.Features.TrackInformation;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using MoreLinq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.TrackInformation
{
    public static class Converter
    {
        private static readonly SolidColorBrush YellowColour = new SolidColorBrush(Color.FromArgb(255, 255, 192, 0));
        private static readonly SolidColorBrush RedColour = new SolidColorBrush(Color.FromArgb(255, 221, 48, 57));
        private static readonly SolidColorBrush GreenColour = new SolidColorBrush(Color.FromArgb(255, 112, 173, 71));
        private static readonly SolidColorBrush GreyColour = new SolidColorBrush(Color.FromArgb(255, 103, 103, 103));

        private static readonly Thickness NoMargin = new Thickness(0, 0, 0, 0);
        private static readonly Thickness DecreasedMargin = new Thickness(0, 1, 0, 0);

        public static string ToSymbol(ListingStatus status) => status switch
        {
            ListingStatus.New => "\xE7C1",
            ListingStatus.Decreased => "\xE96E",
            ListingStatus.Increased => "\xE96D",
            ListingStatus.Unchanged => "\xE94E",
            ListingStatus.Back => "\xE248",
            _ => "\xE949",
        };

        public static SolidColorBrush ToSymbolColour(ListingStatus status) => status switch
        {
            ListingStatus.New => YellowColour,
            ListingStatus.Back => YellowColour,
            ListingStatus.Decreased => RedColour,
            ListingStatus.Increased => GreenColour,
            _ => GreyColour,
        };

        public static Thickness ToMargin(ListingStatus status) => status switch
        {
            ListingStatus.Decreased => DecreasedMargin,
            _ => NoMargin,
        };

        public static string MakePositive(int? value)
        {
            if (value is null || value == 0) return string.Empty;

            return (value < 0)
                    ? (value * -1).ToString()
                    : value.ToString();
        }

        public static string PositionString(int? value)
        {
            return value is null ? "-" : value.ToString();
        }

        public static string MakeDateTimeString(DateTime? value)
        {
            if (value is null) return string.Empty;

            var hour = value.Value.Hour;
            var untilHour = hour + 1;
            if (untilHour > 24)
                untilHour = 0;

            var date = value.Value.ToString("dddd d MMMM yyyy");

            return $"{date} {hour}:00 - {untilHour}:00";
        }

        public static Visibility HideWhenNotListed(ListingStatus status)
        {
            return status == ListingStatus.NotListed
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }

    public sealed partial class TrackInformationPage : Page
    {
        public TrackInformationPage()
        {
            this.InitializeComponent();
        }

        public TrackInformationViewModel ViewModel { get; set; }

        public void OnLeftClick()
        {
        }

        public void OnRightClick()
        {
        }

        public async Task LoadNewTrackAsync(TrackListing listing)
        {
            await ViewModel.LoadTrackDetails(listing);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var selectedTrackListing = (TrackListing)e.Parameter;

            ViewModel = App.GetService<TrackInformationViewModel>();
            await ViewModel.LoadTrackDetails(selectedTrackListing);
        }
    }

    public class TrackInformationViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public TrackInformationViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public string Title
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public string Artist
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public int RecordedYear
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public ObservableCollection<ListingInformation> Listings { get; } = new ObservableCollection<ListingInformation>();

        public ListingInformation Highest
        {
            get { return GetPropertyValue<ListingInformation>(); }
            set { SetPropertyValue(value); }
        }

        public ListingInformation Lowest
        {
            get { return GetPropertyValue<ListingInformation>(); }
            set { SetPropertyValue(value); }
        }

        public ListingInformation Latest
        {
            get { return GetPropertyValue<ListingInformation>(); }
            set { SetPropertyValue(value); }
        }

        public ListingInformation First
        {
            get { return GetPropertyValue<ListingInformation>(); }
            set { SetPropertyValue(value); }
        }

        public int Appearances
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public bool IsLatestListed
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public int AppearancesPossible
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public async Task LoadTrackDetails(TrackListing trackListing)
        {
            var track = await mediator.Send(new TrackInformationRequest(trackListing.TrackId));
            Title = track.Title;
            Artist = track.Artist;
            RecordedYear = track.RecordedYear;
            Highest = track.Highest;
            Lowest = track.Lowest;
            Latest = track.Latest;
            First = track.First;
            Appearances = track.Appearances;
            AppearancesPossible = track.AppearancesPossible;
            IsLatestListed = track.Listings.First().Status != ListingStatus.NotListed;
            Listings.Clear();
            foreach (var item in track.Listings)
            {
                Listings.Add(item);
            }
        }
    }
}
