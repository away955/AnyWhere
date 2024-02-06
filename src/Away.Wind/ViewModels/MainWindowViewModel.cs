namespace Away.Wind;

public class MainWindowViewModel : BindableBase, INavigationAware
{
    private readonly IDialogService _dialogService;
    private readonly IRegionManager _regionManager;
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly ISettingsRepository _settingsRepository;
    private readonly IMenuRepository _menuRepository;

    public MainWindowViewModel(
        IDialogService dialogService,
        IRegionManager regionManager,
        ILogger<MainWindowViewModel> logger,
        ISettingsRepository settingsRepository,
        IMenuRepository menuRepository
        )
    {
        _dialogService = dialogService;
        _regionManager = regionManager;
        _logger = logger;
        _settingsRepository = settingsRepository;
        _menuRepository = menuRepository;

        NavigateCommand = new DelegateCommand<string>(Navigate);
        MenuToggleChangeCommand = new DelegateCommand<bool?>(MenuToggleChange);
        Init();
    }


    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        Init();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        Init();
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {

    }

    private void Init()
    {
        Icon = _settingsRepository.GetValue("Icon");
        Logo = _settingsRepository.GetValue("Logo");
        LogoTitle = _settingsRepository.GetValue("LogoTitle");

        var menus = _menuRepository.GetList()
            .Select(o => new ItemMenuModel { Icon = o.Icon!, Title = o.Title!, URL = o.Url! });

        LeftMenus = new ObservableCollection<ItemMenuModel>(menus);
        TopMenus = [
            new TopMenuModel
            {
                Icon = "Settings",
                ToolTip = "系统代理",
                Command = new DelegateCommand(() => _dialogService.Show("system-proxy")),
            },
            new TopMenuModel
            {
                Icon = "Palette",
                ToolTip = "皮肤设置",
                Command = new DelegateCommand(() => _dialogService.Show("system-theme-settings"))
            },
            new TopMenuModel
            {
                Icon = "Close",
                ToolTip = "关闭",
                Command = new DelegateCommand(OnExit),
            },
        ];
    }


    private string? _icon;
    /// <summary>
    /// 任务栏Logo
    /// </summary>
    public string? Icon
    {
        get { return _icon; }
        set { SetProperty(ref _icon, value); }
    }

    private string? _logo;
    /// <summary>
    /// 菜单Logo
    /// </summary>
    public string? Logo
    {
        get { return _logo; }
        set { SetProperty(ref _logo, value); }
    }

    private string? _logoTitle;
    /// <summary>
    /// Logo标题
    /// </summary>
    public string? LogoTitle { get => _logoTitle; set => SetProperty(ref _logoTitle, value); }

    private ObservableCollection<ItemMenuModel>? _leftMenus;
    public ObservableCollection<ItemMenuModel>? LeftMenus { get => _leftMenus; set => SetProperty(ref _leftMenus, value); }

    public DelegateCommand<string> NavigateCommand { get; private set; }
    private void Navigate(string navigatePath)
    {
        if (navigatePath == null)
        {
            return;
        }
        _regionManager.RequestNavigate("MainBox", navigatePath);
    }


    private bool _menuToggle;
    /// <summary>
    /// 菜单是否展开
    /// </summary>
    public bool MenuToggle { get => _menuToggle; set => SetProperty(ref _menuToggle, value); }

    public DelegateCommand<bool?> MenuToggleChangeCommand { get; private set; }
    /// <summary>
    /// 展开|收起菜单
    /// </summary>
    /// <param name="show"></param>
    private void MenuToggleChange(bool? show)
    {
        MenuToggle = show ?? false;
    }


    private ObservableCollection<TopMenuModel> _topMenus = [];
    public ObservableCollection<TopMenuModel> TopMenus { get => _topMenus; set => SetProperty(ref _topMenus, value); }

    private void OnExit()
    {
        Application.Current.Shutdown();
    }


}