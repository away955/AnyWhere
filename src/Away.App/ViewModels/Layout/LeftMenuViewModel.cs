namespace Away.App.ViewModels;

public sealed class LeftMenuViewModel : ViewModelBase
{
    public static string Title => Constant.Title;
    public static string Version => Constant.Version;

    private readonly IAppMapper _mapper;
    private readonly IAppMenuRepository _appMenuRepository;

    /// <summary>
    /// 展开|收起菜单
    /// </summary>
    public ICommand MenuToggleCommand { get; }

    private string _checkedMenu = string.Empty;
    /// <summary>
    /// 选中的菜单
    /// </summary>
    public string CheckedMenu
    {
        get => _checkedMenu;
        set
        {
            this.RaiseAndSetIfChanged(ref _checkedMenu, value);
            MessageRouter.Go(_checkedMenu);
        }
    }

    /// <summary>
    /// 是否展开菜单
    /// </summary>
    [Reactive]
    public bool IsMenuToggle { get; set; }
    /// <summary>
    /// 菜单列表
    /// </summary>
    [Reactive]
    public ObservableCollection<AppMenuModel> Menus { get; set; } = [];

    public LeftMenuViewModel(IAppMenuRepository appMenuRepository, IAppMapper mapper)
    {
        _mapper = mapper;
        _appMenuRepository = appMenuRepository;

        MenuToggleCommand = ReactiveCommand.Create(OnMenuToggle);
        Init();
        MessageEvent.Listen(o => Init(), "RefreshMenu");
    }
    protected override void OnActivation()
    {
        Init();
        CheckedMenu = "app-store";
    }

    private void Init()
    {
        var items = _appMenuRepository.GetList().Select(_mapper.Map<AppMenuModel>);
        Menus = new ObservableCollection<AppMenuModel>(items);
    }

    private void OnMenuToggle()
    {
        IsMenuToggle = !IsMenuToggle;
    }


}
