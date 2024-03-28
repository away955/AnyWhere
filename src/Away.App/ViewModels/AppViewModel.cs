namespace Away.App.ViewModels;

[ViewModel]
public sealed class AppViewModel : ViewModelBase
{
    public static string Title => $"{AppInfo.Title} {AppInfo.Version}";

    public ICommand ExitCommand { get; }
    public ICommand ShowCommand { get; }

    public AppViewModel()
    {
        ExitCommand = ReactiveCommand.Create(OnExitCommand);
        ShowCommand = ReactiveCommand.Create(OnShowCommand);
    }
    private void OnExitCommand()
    {
        MessageBus.Current.Publish(MessageBusType.Shutdown, new object());
    }

    private static void OnShowCommand()
    {
        MessageBus.Current.Publish(MessageBusType.WindowState, WindowStateCommandType.ShowActivate);
    }
}
