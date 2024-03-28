using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using ReactiveUI;

namespace Away.App.Core.Messages;

public sealed class MessageShow
{
    public void Success(string title, string? message = "")
    {
        Show(title, message, NotificationType.Success);
    }
    public void Error(string title, string? message = "")
    {
        Show(title, message, NotificationType.Error);
    }
    public void Information(string title, string? message = "")
    {
        Show(title, message, NotificationType.Information);
    }
    public void Warning(string title, string? message = "")
    {
        Show(title, message, NotificationType.Warning);
    }
    public void Show(string? title, string? message = "", NotificationType type = NotificationType.Information, TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        Log.Information($"{title}\r\n{message}");
        Dispatcher.UIThread.Post(() =>
        {
            MessageBus.Current.Nofity(title, message, type, expiration, onClick, onClose);
        });
    }
}
