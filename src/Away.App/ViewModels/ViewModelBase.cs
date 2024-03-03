namespace Away.App.ViewModels;

public abstract class ViewModelBase : ReactiveObject
{
    protected static void Show(string message, NotificationType notificationType = NotificationType.Information)
    {
        Log.Information(message);
        MessageBus.Current.Nofity(string.Empty, message, notificationType);
    }
}
