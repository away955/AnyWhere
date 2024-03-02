namespace Away.Wind.ViewModels;

public class LeftMenuVM : BindableBase
{
    private const string RegionName = "MainBox";
    private readonly IRegionManager _regionManager;
    private readonly ISettingsRepository _settingsRepository;
    private readonly IMenuRepository _menuRepository;


    public LeftMenuVM(
        IRegionManager regionManager,
        ISettingsRepository settingsRepository,
        IMenuRepository menuRepository
        )
    {
        _regionManager = regionManager;
        _settingsRepository = settingsRepository;
        _menuRepository = menuRepository;

        SelectionChangedCommand = new(OnSelectionChangedCommand);

        Logo = _settingsRepository.GetValue("Logo");
        LogoTitle = _settingsRepository.GetValue("LogoTitle");

        var menus = _menuRepository.GetList().Select(o => new ItemMenuModel { Icon = o.Icon, Title = o.Title, URL = o.Url });
        MenuItemSource = new ObservableCollection<ItemMenuModel>(menus);
    }

    private string _logo = string.Empty;
    /// <summary>
    /// 菜单Logo
    /// </summary>
    public string Logo
    {
        get { return _logo; }
        set { SetProperty(ref _logo, value); }
    }

    private string _logoTitle = string.Empty;
    /// <summary>
    /// Logo标题
    /// </summary>
    public string LogoTitle
    {
        get => _logoTitle; set => SetProperty(ref _logoTitle, value);
    }

    private ObservableCollection<ItemMenuModel> _menuItemSource = [];
    /// <summary>
    /// 菜单列表
    /// </summary>
    public ObservableCollection<ItemMenuModel> MenuItemSource
    {
        get => _menuItemSource;
        set => SetProperty(ref _menuItemSource, value);
    }

    /// <summary>
    /// 选中菜单
    /// </summary>
    public DelegateCommand<SelectionChangedEventArgs?> SelectionChangedCommand { get; private set; }
    private void OnSelectionChangedCommand(SelectionChangedEventArgs? e)
    {
        if (e == null || e.AddedItems[0] is not ItemMenuModel model)
        {
            return;
        }
        _regionManager.RequestNavigate(RegionName, model.URL);
    }

    /// <summary>
    /// 默认选中
    /// </summary>
    public string DefaultUrl { get; set; } = string.Empty;
    public void SetDefaultMenu()
    {
        _regionManager.RequestNavigate(RegionName, DefaultUrl);
    }

    private bool _toggle;
    /// <summary>
    /// 展开|收起菜单
    /// </summary>
    public bool Toggle
    {
        get => _toggle;
        set => SetProperty(ref _toggle, value);
    }


}

public class ItemMenuModel : BindableBase
{
    private string _title = string.Empty;
    private string _icon = string.Empty;
    private string _url = string.Empty;

    public required string Title { get => _title; set => SetProperty(ref _title, value); }
    public required string Icon { get => _icon; set => SetProperty(ref _icon, value); }
    public required string URL { get => _url; set => SetProperty(ref _url, value); }
}