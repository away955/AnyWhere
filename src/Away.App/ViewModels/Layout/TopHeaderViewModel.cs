namespace Away.App.ViewModels;

public sealed class TopHeaderViewModel : ViewModelBase
{
    private static readonly string Maximum = "&#xe606;".ToUnicode();
    private static readonly string Normal = "&#xe6b7;".ToUnicode();

    private readonly IVersionService _versionService;
    private readonly IAppThemeService _appThemeService;

    public ICommand CloseCommand { get; }
    public ICommand NormalCommand { get; }
    public ICommand MinimizedCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand InfoCommand { get; }

    public ICommand BackCommand { get; }
    public ICommand NextCommand { get; }

    public bool IsBack => MessageRouter.HasBack();
    public bool IsNext => MessageRouter.HasNext();


    [Reactive]
    public string NormalIcon { get; set; } = Maximum;

    [Reactive]
    public string UpdateHeader { get; set; } = string.Empty;

    [Reactive]
    public bool IsEnabled { get; set; }

    private bool _isDefaultTheme = true;
    public bool IsDefaultTheme
    {
        get => _isDefaultTheme;
        set
        {
            this.RaiseAndSetIfChanged(ref _isDefaultTheme, value);
            if (_isDefaultTheme)
            {
                _appThemeService.Set(ThemeType.Default);
            }
        }
    }
    private bool _isLightTheme = false;
    public bool IsLightTheme
    {
        get => _isLightTheme;
        set
        {
            this.RaiseAndSetIfChanged(ref _isLightTheme, value);
            _appThemeService.Set(_isLightTheme ? ThemeType.Light : ThemeType.Dark);
        }
    }

    public TopHeaderViewModel(
        IAppThemeService appThemeService,
        IVersionService versionService)
    {
        _versionService = versionService;
        _appThemeService = appThemeService;

        CloseCommand = ReactiveCommand.Create(() => OnCommand(WindowStateCommandType.Hide));
        MinimizedCommand = ReactiveCommand.Create(() => OnCommand(WindowStateCommandType.Minimized));
        NormalCommand = ReactiveCommand.Create(OnNomalCommand);
        UpdateCommand = ReactiveCommand.Create(OnUpdateCommand);
        InfoCommand = ReactiveCommand.Create(OnInfoCommand);

        BackCommand = ReactiveCommand.Create(MessageRouter.Back);
        NextCommand = ReactiveCommand.Create(MessageRouter.Next);
        MessageRouter.Listen(args =>
        {
            if (args is not string url)
            {
                return;
            }
            this.RaisePropertyChanged(nameof(IsBack));
            this.RaisePropertyChanged(nameof(IsNext));
        });

        _ = Init();
    }

    /// <summary>
    /// App信息
    /// </summary>
    private AppResource? AppResource { get; set; }
    /// <summary>
    /// 更新程序信息
    /// </summary>
    private AppResource? UpdateResource { get; set; }
    private async Task Init()
    {
        (AppResource, UpdateResource) = await _versionService.GetAppResource();
        if (AppResource == null)
        {
            return;
        }
        var hasNewVersion = AppResource.HasNewVersion(Constant.Version.Replace("v", string.Empty));
        if (hasNewVersion)
        {
            IsEnabled = true;
            UpdateHeader = $"检查更新(v{AppResource.Version})";
            MessageShow.Info($"有新版本：v{AppResource.Version}", AppResource.Description);
        }
        else
        {
            IsEnabled = false;
            UpdateHeader = $"检查更新";
        }

        var theme = _appThemeService.Get();
        IsDefaultTheme = theme == ThemeType.Default;
        if (!IsDefaultTheme)
        {
            IsLightTheme = theme == ThemeType.Light;
        }

        // 检查更新程序
        _ = InstallUpdate();
    }

    private void OnInfoCommand()
    {
        var title = $"{Constant.Title} {Constant.Version}";
        var dest = "一款绿色的网络代理软件\naway©2024-03 至今";
        MessageShow.Info(title, dest);
    }

    private async void OnUpdateCommand()
    {
        IsEnabled = false;
        var updateExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Away.App.Update");
        List<string> args = [];
        if (AppResource != null)
        {
            var url = await _versionService.GetDownloadRequest(AppResource.ContentID);
            args.Add(url);
            args.Add(AppResource.Updated);
            args.Add(AppResource.Version);
            args.Add(AppResource.Description);
        }
        await foreach (var cmdEvent in Cli.Wrap(updateExe).WithArguments(args).ListenAsync())
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

    /// <summary>
    /// 检查更新更新程序
    /// </summary>
    /// <returns></returns>
    private async Task InstallUpdate()
    {
        if (UpdateResource == null)
        {
            return;
        }
        var filename = OperatingSystem.IsWindows() ? "Away.App.Update.exe" : "Away.App.Update";
        var update = System.Diagnostics.FileVersionInfo.GetVersionInfo(Path.Combine(Constant.RootPath, filename));
        var hasUpdateNewVersion = UpdateResource.HasNewVersion(update!.FileVersion!);
        if (!hasUpdateNewVersion)
        {
            return;
        }

        // 下载
        var zipPath = Path.Combine(Constant.RootPath, "tmp", "update.zip");
        var flag = await _versionService.DownloadFile(UpdateResource.ContentID, zipPath);
        if (!flag)
        {
            return;
        }

        // 解压安装
        using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read, System.Text.Encoding.Default))
        {
            archive.ExtractToDirectory(Constant.RootPath, true);
        }
        File.Delete(zipPath);
    }
}
