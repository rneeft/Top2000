using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Chroomsoft.Top2000.Apps.Views;

internal sealed class FavoriteChangedMessage : ValueChangedMessage<int>
{
    public FavoriteChangedMessage(int value) : base(value)
    {
    }

    public required object Sender { get; init; }
    public required bool IsFavorite { get; init; }
}