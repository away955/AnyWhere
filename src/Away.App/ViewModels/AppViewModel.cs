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
        MessageShutdown.Shutdown();
    }

    private static void OnShowCommand()
    {
        MessageWindowState.State(WindowStateCommandType.ShowActivate);
    }
}
