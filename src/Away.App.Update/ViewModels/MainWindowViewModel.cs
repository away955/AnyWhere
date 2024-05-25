using Avalonia.Controls.ApplicationLifetimes;

namespace Away.App.Update.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    public static string Title => $"{AppInfo.Title} {AppInfo.Version}";
    private string DownloadUrl { get; set; } = string.Empty;

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

    public MainWindowViewModel(IUpgradeService updateService)
    {
        _updateService = updateService;

        CloseCommand = ReactiveCommand.Create(() => OnCommand("Close"));
        MinimizedCommand = ReactiveCommand.Create(() => OnCommand("Minimized"));
        UpdateCommand = ReactiveCommand.Create(OnUpdateCommand);
        CancelCommand = ReactiveCommand.Create(OnCancelCommand);

        _updateService.OnDownloadProgress += OnDownloadProgress;
        _updateService.OnInstallProgress += OnInstallProgress;
        _updateService.OnError += OnError;
        Init();
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

    private void Init()
    {
        if (Application.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktopStyleApplicationLifetime)
        {
            return;
        }
        if (desktopStyleApplicationLifetime.Args!.Length != 4)
        {
            return;
        }
        Log.Information("{0}", desktopStyleApplicationLifetime.Args);
        DownloadUrl = desktopStyleApplicationLifetime.Args[0];
        Updated = desktopStyleApplicationLifetime.Args[1];
        Version = "v" + desktopStyleApplicationLifetime.Args[2];
        Info = desktopStyleApplicationLifetime.Args[3];
        IsShowInfo = !string.IsNullOrWhiteSpace(Version);
        IsEnable = true;
    }

    private static void OnCommand(string cmd)
    {
        MessageBus.Current.SendMessage(cmd);
    }

}
