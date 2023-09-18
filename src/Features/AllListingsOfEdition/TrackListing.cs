using System.ComponentModel;

namespace Chroomsoft.Top2000.Features.AllListingsOfEdition;

public class TrackListing : INotifyPropertyChanged
{
    private bool isFavorite;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int TrackId { get; set; }

    public int Position { get; set; }

    public int? Delta { get; set; }

    public DateTime LocalPlayDateAndTime => DateTime.SpecifyKind(PlayUtcDateAndTime, DateTimeKind.Utc).ToLocalTime();

    public DateTime PlayUtcDateAndTime { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;

    public bool IsFavorite
    {
        get
        {
            return isFavorite;
        }
        set
        {
            isFavorite = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFavorite)));
        }
    }
}