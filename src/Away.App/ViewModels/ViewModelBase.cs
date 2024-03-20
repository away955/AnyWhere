using System.Reactive.Disposables;

namespace Away.App.ViewModels;

public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }

    public ViewModelBase()
    {
        Activator = new ViewModelActivator();
        this.WhenActivated(disposables =>
        {
            OnActivation();
            Disposable.Create(OnDeactivation).DisposeWith(disposables);
        });
    }

    protected virtual void OnActivation()
    {

    }

    protected virtual void OnDeactivation()
    {

    }


    protected static void Success(string title, string? message = "")
    {
        Show(title, message, NotificationType.Success);
    }
    protected static void Error(string title, string? message = "")
    {
        Show(title, message, NotificationType.Error);
    }
    protected static void Information(string title, string? message = "")
    {
        Show(title, message, NotificationType.Information);
    }
    protected static void Warning(string title, string? message = "")
    {
        Show(title, message, NotificationType.Warning);
    }
    protected static void Show(string? title, string? message = "", NotificationType type = NotificationType.Information, TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        Log.Information($"{title}\r\n{message}");
        Dispatcher.UIThread.Post(() =>
        {
            MessageBus.Current.Nofity(title, message, type, expiration, onClick, onClose);
        });
    }
}
