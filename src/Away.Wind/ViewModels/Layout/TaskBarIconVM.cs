namespace Away.Wind.ViewModels;

public class TaskBarIconVM : BindableBase
{
    private readonly ISettingsRepository _settingsRepository;
    private readonly IXrayService _xrayService;

    public TaskBarIconVM(ISettingsRepository settingsRepository, IXrayService xrayService)
    {
        _settingsRepository = settingsRepository;
        _xrayService = xrayService;

        ShutdownCommand = new(OnShutdownCommand);
        ShowCommand = new(OnShowCommand);

        Icon = _settingsRepository.GetValue("Icon");
    }


    private string _icon = string.Empty;
    /// <summary>
    /// 任务栏图标
    /// </summary>
    public string Icon
    {
        get { return _icon; }
        set { SetProperty(ref _icon, value); }
    }

    /// <summary>
    /// 关闭程序
    /// </summary>
    public DelegateCommand ShutdownCommand { get; private set; }
    private bool _isClose;
    public bool IsClose
    {
        get => _isClose;
        set => SetProperty(ref _isClose, value);
    }
    private void OnShutdownCommand()
    {
        _xrayService.CloseAll();
        IsClose = true;
    }

    /// <summary>
    /// 显示主窗口
    /// </summary>
    public DelegateCommand ShowCommand { get; private set; }
    public bool _isShow;
    public bool IsShow
    {
        get => _isShow;
        set => SetProperty(ref _isShow, value);
    }
    private void OnShowCommand()
    {
        IsShow = true;
        RaisePropertyChanged(nameof(IsShow));
    }

}
