namespace Away.Wind.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;

    private string _icon = "pack://application:,,,/Away.Wind;component/Assets/favicon.ico";
    public string Icon
    {
        get { return _icon; }
        set { SetProperty(ref _icon, value); }
    }

    public MainWindowViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        NavigateCommand = new DelegateCommand<string>(Navigate);
        MenuToggleChangeCommand = new DelegateCommand<bool?>(MenuToggleChange);
    }


    #region Left Menu Settings
    private string _logo = "pack://application:,,,/Away.Wind;component/Assets/logo_dark.png";
    public string Logo
    {
        get { return _logo; }
        set { SetProperty(ref _logo, value); }
    }

    private string _logoTitle = "俊哥出品";
    public string LogoTitle { get => _logoTitle; set => SetProperty(ref _logoTitle, value); }

    private ObservableCollection<MenuModel> _leftMenus = [
        new MenuModel
        {
            Icon = "CogOutline",
            Title = "职业",
            URL = "404"
        },
        new MenuModel
        {
            Icon = "Home",
            Title = "主页",
            URL = "menu-settings"
        },
        new MenuModel
        {
            Icon = "Chat",
            Title = "message",
            URL = "settings"
        },
        new MenuModel
        {
            Icon = "Account",
            Title = "map",
            URL = "settings"
        },
        new MenuModel
        {
            Icon = "CogOutline",
            Title = "职业",
            URL = "404"
        },
        new MenuModel
        {
            Icon = "Home",
            Title = "主页",
            URL = "menu-settings"
        },
        new MenuModel
        {
            Icon = "Chat",
            Title = "message",
            URL = "settings"
        },
        new MenuModel
        {
            Icon = "Account",
            Title = "map",
            URL = "settings"
        },
        new MenuModel
        {
            Icon = "CogOutline",
            Title = "职业",
            URL = "404"
        },
        new MenuModel
        {
            Icon = "Home",
            Title = "主页",
            URL = "menu-settings"
        },
        new MenuModel
        {
            Icon = "Chat",
            Title = "message",
            URL = "settings"
        },
        new MenuModel
        {
            Icon = "Account",
            Title = "map",
            URL = "settings"
        },
        new MenuModel
        {
            Icon = "CogOutline",
            Title = "职业",
            URL = "404"
        },
        new MenuModel
        {
            Icon = "Home",
            Title = "主页",
            URL = "menu-settings"
        },
        new MenuModel
        {
            Icon = "Chat",
            Title = "message",
            URL = "settings"
        },
        new MenuModel
        {
            Icon = "Account",
            Title = "map",
            URL = "settings"
        }
    ];
    public ObservableCollection<MenuModel> LeftMenus { get => _leftMenus; set => SetProperty(ref _leftMenus, value); }

    public DelegateCommand<string> NavigateCommand { get; private set; }
    private void Navigate(string navigatePath)
    {
        if (navigatePath == null)
        {
            return;
        }
        _regionManager.RequestNavigate("ContentRegion", navigatePath);
    }


    private bool _menuToggle;
    public bool MenuToggle { get => _menuToggle; set => SetProperty(ref _menuToggle, value); }

    #endregion

    #region Top Header

    public DelegateCommand<bool?> MenuToggleChangeCommand { get; private set; }
    private void MenuToggleChange(bool? show)
    {
        MenuToggle = show ?? false;
    }


    private ObservableCollection<TopMenuModel> _topMenus = [
        new TopMenuModel
        {
            Icon = "Settings",
            ToolTip = "设置",
            Command = new DelegateCommand(() =>
            {
                MessageBox.Show("设置");
            }),
        },
        new TopMenuModel
        {
            Icon = "CloseBox",
            ToolTip = "关闭",
            Command = new DelegateCommand(OnExit),
        },
    ];
    public ObservableCollection<TopMenuModel> TopMenus { get => _topMenus; set => SetProperty(ref _topMenus, value); }

    #endregion


    private static void OnExit()
    {
        Application.Current.Shutdown();
    }
}