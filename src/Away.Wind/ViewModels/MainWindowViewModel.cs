namespace Away.Wind.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;

    private string _logo = "pack://application:,,,/Away.Wind;component/Assets/logo.png";
    public string Logo
    {
        get { return _logo; }
        set { SetProperty(ref _logo, value); }
    }

    private string _title = "Prism Unity Application";
    public string Title
    {
        get { return _title; }
        set { SetProperty(ref _title, value); }
    }

    public MainWindowViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        NavigateCommand = new DelegateCommand<string>(Navigate);
        MenuToggleChangeCommand = new DelegateCommand<bool?>(MenuToggleChange);
    }

    #region Left Menu Settings

    private string _logoTitle = "Away DeskTop";
    public string LogoTitle { get => _logoTitle; set => SetProperty(ref _logoTitle, value); }

    private ObservableCollection<MenuModel> _leftMenus = [
        new MenuModel
        {
            Icon = "CogOutline",
            Title = "home",
            URL = "404"
        },
        new MenuModel
        {
            Icon = "Home",
            Title = "menu settings",
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
            Title = "home",
            URL = "404"
        },
        new MenuModel
        {
            Icon = "Home",
            Title = "menu settings",
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
            Title = "home",
            URL = "404"
        },
        new MenuModel
        {
            Icon = "Home",
            Title = "menu settings",
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
            Title = "home",
            URL = "404"
        },
        new MenuModel
        {
            Icon = "Home",
            Title = "menu settings",
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

    public DelegateCommand<bool?> MenuToggleChangeCommand { get; private set; }
    private void MenuToggleChange(bool? show)
    {
        MenuToggle = show ?? false;
    }

    #endregion


    private bool _menuToggle;
    public bool MenuToggle { get => _menuToggle; set => SetProperty(ref _menuToggle, value); }
}