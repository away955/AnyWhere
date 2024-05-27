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
    /// <summary>
    /// 选中的菜单
    /// </summary>
    [Reactive]
    public string CheckedMenu { get; set; } = string.Empty;

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

    public LeftMenuViewModel(
        IAppMenuRepository appMenuRepository,
        IAppMapper mapper)
    {
        _mapper = mapper;
        _appMenuRepository = appMenuRepository;

        MenuToggleCommand = ReactiveCommand.Create(OnMenuToggle);
        Init();
        MessageEvent.Listen(o => Init(), "RefreshMenu");
        ViewRouter.Listen(args =>
        {
            if (args.Path != CheckedMenu)
            {
                CheckedMenu = args.Path;
            }
        });
    }

    protected override void OnActivation()
    {
        Init();

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
