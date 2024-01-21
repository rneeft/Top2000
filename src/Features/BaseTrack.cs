using System.ComponentModel;

namespace Chroomsoft.Top2000.Features;

public class BaseTrack : INotifyPropertyChanged
{
    private bool isFavorite;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int Id { get; init; }

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