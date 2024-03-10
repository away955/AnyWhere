using Away.App.Update.Services;
using ReactiveUI.Fody.Helpers;
using System.Threading.Tasks;

namespace Away.App.Update.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly IVersionService _versionService;

    public ICommand CloseCommand { get; }
    public ICommand MinimizedCommand { get; }

    [Reactive]
    public string Updated { get; set; } = string.Empty;
    [Reactive]
    public string Version { get; set; } = string.Empty;
    [Reactive]
    public string Info { get; set; } = string.Empty;

    public MainWindowViewModel(IVersionService versionService)
    {
        _versionService = versionService;

        CloseCommand = ReactiveCommand.Create(() => OnCommand("Close"));
        MinimizedCommand = ReactiveCommand.Create(() => OnCommand("Minimized"));


        _ = Init();
    }

    private async Task Init()
    {
        var versionInfo = await _versionService.GetVersionInfo();
        if (versionInfo == null)
        {
            return;
        }
        Updated = versionInfo.Updated;
        Version = versionInfo.Version;
        Info = versionInfo.Info;
    }

    private static void OnCommand(string cmd)
    {
        MessageBus.Current.SendMessage(cmd);
    }

}
