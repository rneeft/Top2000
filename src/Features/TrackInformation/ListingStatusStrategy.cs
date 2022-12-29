﻿namespace Chroomsoft.Top2000.Features.TrackInformation;

public class ListingStatusStrategy
{
    private readonly int recordedYear;
    private readonly List<Listing> previous = new();

    public ListingStatusStrategy(int recordedYear)
    {
        this.recordedYear = recordedYear;
    }

    public ListingStatus Determine(Listing current)
    {
        var status = GetStatus(current);
        previous.Add(current);
        return status;
    }

    private ListingStatus GetStatus(Listing current)
    {
        if (!current.CouldBeListed(recordedYear))
        {
            return ListingStatus.NotAvailable;
        }

        if (!current.Position.HasValue)
        {
            return ListingStatus.NotListed;
        }

        if (!previous.Any(x => x.Status == ListingStatus.New))
        {
            return ListingStatus.New;
        }

        if (!current.Offset.HasValue)
        {
            return ListingStatus.Back;
        }

        if (current.Offset == 0)
        {
            return ListingStatus.Unchanged;
        }

        if (current.Offset < 0)
        {
            return ListingStatus.Increased;
        }

        if (current.Offset > 0)
        {
            return ListingStatus.Decreased;
        }

        // rather return unknown than an exception
        return ListingStatus.Unknown;
    }
}
