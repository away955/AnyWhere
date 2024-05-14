namespace Away.App.Update.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private const string InfoUrl = AppInfo.InfoUrl;
    private const string DownloadUrl = AppInfo.DownloadUrl;
    public static string Title => $"{AppInfo.Title} {AppInfo.Version}";

    private readonly IVersionService _versionService;
    private readonly IUpgradeService _updateService;

    public ICommand CloseCommand { get; }
    public ICommand MinimizedCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand CancelCommand { get; }

    [Reactive]
    public string Updated { get; set; } = string.Empty;
    [Reactive]
    public string Version { get; set; } = string.Empty;
    [Reactive]
    public string Info { get; set; } = string.Empty;
    [Reactive]
    public bool IsShowInfo { get; set; }

    [Reactive]
    public string DownloadDest { get; set; } = string.Empty;
    [Reactive]
    public int DownloadProgressValue { get; set; }
    [Reactive]
    public string InstallDest { get; set; } = string.Empty;
    [Reactive]
    public int InstallProgressValue { get; set; }
    [Reactive]
    public string ErrorMessage { get; set; } = string.Empty;

    [Reactive]
    public bool IsEnable { get; set; }

    public MainWindowViewModel(IVersionService versionService, IUpgradeService updateService)
    {
        _versionService = versionService;
        _updateService = updateService;

        CloseCommand = ReactiveCommand.Create(() => OnCommand("Close"));
        MinimizedCommand = ReactiveCommand.Create(() => OnCommand("Minimized"));
        UpdateCommand = ReactiveCommand.Create(OnUpdateCommand);
        CancelCommand = ReactiveCommand.Create(OnCancelCommand);

        _updateService.OnDownloadProgress += OnDownloadProgress;
        _updateService.OnInstallProgress += OnInstallProgress;
        _updateService.OnError += OnError;
        _ = Init();
    }

    private void OnError(string error)
    {
        ErrorMessage = error;
    }

    private void OnInstallProgress(UpdatelEventArgs e)
    {
        InstallDest = e.Description;
        InstallProgressValue = e.ProgressValue;
        if (InstallProgressValue == 100)
        {
            InstallDest = "安装完成";
            IsEnable = true;
        }
    }

    private void OnDownloadProgress(UpdatelEventArgs e)
    {
        DownloadDest = e.Description;
        DownloadProgressValue = e.ProgressValue;
        if (DownloadProgressValue == 100)
        {
            DownloadDest = "下载完成";
        }
    }

    private void OnCancelCommand()
    {
        _updateService.Cancel();
        IsEnable = true;
    }

    private void OnUpdateCommand()
    {
        IsEnable = false;
        ErrorMessage = string.Empty;
        _updateService.Start(DownloadUrl);
    }

    private async Task Init()
    {
        var versionInfo = await _versionService.GetVersionInfo(InfoUrl);
        if (versionInfo == null)
        {
            return;
        }
        Updated = versionInfo.Updated;
        Version = "v" + versionInfo.Version;
        Info = versionInfo.Info;
        IsShowInfo = !string.IsNullOrWhiteSpace(Version);
        IsEnable = true;
    }

    private static void OnCommand(string cmd)
    {
        MessageBus.Current.SendMessage(cmd);
    }

}
