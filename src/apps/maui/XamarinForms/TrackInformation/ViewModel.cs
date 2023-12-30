﻿using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Features.TrackInformation;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Apps.TrackInformation
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Listings = new ObservableList<ListingInformation>();
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

        public string ArtistWithYear
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public ObservableList<ListingInformation> Listings { get; }

        public int TotalListings
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

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

        public double AppearancesPossiblePercentage
        {
            get { return GetPropertyValue<double>(); }
            set { SetPropertyValue(value); }
        }

        public double TotalTop2000Percentage
        {
            get { return GetPropertyValue<double>(); }
            set { SetPropertyValue(value); }
        }

        public async Task LoadTrackDetailsAsync(int trackId)
        {
            var track = await mediator.Send(new TrackInformationRequest(trackId));

            Title = track.Title;
            ArtistWithYear = $"{track.Artist} ({track.RecordedYear})";
            Artist = track.Artist;
            Highest = track.Highest;
            Lowest = track.Lowest;
            Latest = track.Latest;
            First = track.First;
            Appearances = track.Appearances;
            AppearancesPossible = track.AppearancesPossible;
            IsLatestListed = track.Listings.First().Status != ListingStatus.NotListed;
            Listings.Clear();
            Listings.ClearAddRange(track.Listings);
            AppearancesPossiblePercentage = (Appearances / (double)AppearancesPossible);
            TotalTop2000Percentage = (Appearances / (double)Listings.Count);
            TotalListings = Listings.Count;
        }
    }
}
