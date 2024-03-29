using CliWrap;
using CliWrap.EventStream;
using System.IO;
using System.Threading.Tasks;

namespace Away.App.ViewModels;

[ViewModel]
public sealed class TopHeaderViewModel : ViewModelBase
{
    private const string CurrentVersion = AppInfo.Version;
    private const string AppInfoUrl = AppInfo.AppInfoUrl;

    private static readonly string Maximum = IconData.Current["Maximum"].ToUnicode();
    private static readonly string Normal = IconData.Current["Normal"].ToUnicode();
    private static readonly string Update = IconData.Current["Update"].ToUnicode();

    private readonly IVersionService _versionService;

    public ICommand CloseCommand { get; }
    public ICommand NormalCommand { get; }
    public ICommand MinimizedCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand InfoCommand { get; }

    [Reactive]
    public string NormalIcon { get; set; } = Maximum;

    [Reactive]
    public string UpdateHeader { get; set; } = string.Empty;

    [Reactive]
    public bool IsEnabled { get; set; }

    public TopHeaderViewModel(IVersionService versionService)
    {
        _versionService = versionService;

        CloseCommand = ReactiveCommand.Create(() => OnCommand(WindowStateCommandType.Hide));
        MinimizedCommand = ReactiveCommand.Create(() => OnCommand(WindowStateCommandType.Minimized));
        NormalCommand = ReactiveCommand.Create(OnNomalCommand);
        UpdateCommand = ReactiveCommand.Create(OnUpdateCommand);
        InfoCommand = ReactiveCommand.Create(OnInfoCommand);

        _ = Init();
    }

    private async Task Init()
    {
        var info = await _versionService.GetVersionInfo(AppInfoUrl);
        if (string.IsNullOrWhiteSpace(info.Version))
        {
            return;
        }
        var hasNewVersion = info.HasNewVersion(CurrentVersion.Replace("v", string.Empty));
        if (hasNewVersion)
        {
            IsEnabled = true;
            UpdateHeader = $"{Update} 检查更新(v{info.Version})";
            MessageShow.Information($"有新版本：{info.Version}", info.Info);
        }
        else
        {
            IsEnabled = false;
            UpdateHeader = $"{Update} 检查更新";
        }
    }

    private void OnInfoCommand()
    {
        var title = $"{AppInfo.Title} {AppInfo.Version}";
        var dest = "一款绿色的网络代理软件\naway©2024-03 至今";
        MessageShow.Information(title, dest);
    }

    private async void OnUpdateCommand()
    {
        IsEnabled = false;
        var updateExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Away.App.Update");
        await foreach (var cmdEvent in Cli.Wrap(updateExe).ListenAsync())
        {
            switch (cmdEvent)
            {
                case StartedCommandEvent:
                    MessageShutdown.Shutdown();
                    break;
            }
        }
    }

    private void OnNomalCommand()
    {
        if (NormalIcon == Maximum)
        {
            OnCommand(WindowStateCommandType.Maximized);
            NormalIcon = Normal;
        }
        else
        {
            OnCommand(WindowStateCommandType.Normal);
            NormalIcon = Maximum;
        }
    }

    private static void OnCommand(WindowStateCommandType state)
    {
        MessageWindowState.State(state);
    }

}
