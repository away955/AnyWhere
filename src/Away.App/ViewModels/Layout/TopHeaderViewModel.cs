namespace Away.App.ViewModels;

public sealed class TopHeaderViewModel : ViewModelBase
{
    private const string CurrentVersion = Constant.Version;
    private const string AppInfoUrl = Constant.AppUpgradeResource;

    private static readonly string Maximum = IconData.Current["Maximum"].ToUnicode();
    private static readonly string Normal = IconData.Current["Normal"].ToUnicode();

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
            UpdateHeader = $"检查更新(v{info.Version})";
            MessageShow.Info($"有新版本：{info.Version}", info.Info);
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
