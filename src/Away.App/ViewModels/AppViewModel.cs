using Away.App.Domain.Setting;

namespace Away.App.ViewModels;

[ViewModel]
public sealed class AppViewModel : ViewModelBase
{
    public static string Title => $"{AppInfo.Title} {AppInfo.Version}";

    public ICommand ExitCommand { get; }
    public ICommand ShowCommand { get; }

    [Reactive]
    public string Theme { get; set; }

    private readonly IAppThemeService _appThemeService;
    public AppViewModel(IAppThemeService appThemeService)
    {
        _appThemeService = appThemeService;
        ExitCommand = ReactiveCommand.Create(OnExitCommand);
        ShowCommand = ReactiveCommand.Create(OnShowCommand);

        Theme = _appThemeService.Get().ToString();
        _appThemeService.Listen(type => Theme = type.ToString());
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
