namespace Away.App.ViewModels;

[ViewModel]
public sealed class XrayRouteViewModel : ViewModelXrayBase
{
    public static List<ComboBoxModel> DomainStrategyItems => [
        new ComboBoxModel
        {
            Text= "AsIs",
            ToolTip="只使用域名进行路由选择，默认值"
        },
        new ComboBoxModel
        {
            Text= "IPIfNonMatch",
            ToolTip="当域名没有匹配任何基于域名的规则时，将域名解析成 IP（A 记录或 AAAA 记录），进行基于 IP 规则的匹配"
        },
        new ComboBoxModel
        {
            Text= "IPOnDemand",
            ToolTip="当匹配时碰到任何基于 IP 的规则，立即将域名解析为 IP 进行匹配。"
        }
        ];

    public static List<ComboBoxModel> DomainMatcherItems => [
        new ComboBoxModel
        {
            Text= "linear",
            ToolTip="使用线性匹配算法，默认值"
        },
        new ComboBoxModel
        {
            Text= "mph",
            ToolTip="使用最小完美散列"
        }
        ];

    [Reactive]
    public XrayRouteModel Route { get; set; } = new();
    public ICommand AddCommand { get; }
    public ICommand DelCommand { get; }

    public XrayRouteViewModel(IXrayService xrayService, IMapper mapper) : base(xrayService, mapper)
    {
        AddCommand = ReactiveCommand.Create(OnAddCommand);
        DelCommand = ReactiveCommand.Create<XrayRouteRuleModel>(OnDelCommand);
    }

    protected override void Init()
    {
        Route = _mapper.Map<XrayRouteModel>(_xrayService.Config.routing ?? new());
    }

    protected override void OnSaveCommand()
    {
        _xrayService.Config.routing = _mapper.Map<XrayRoute>(Route);
        base.OnSaveCommand();
    }


    private void OnAddCommand()
    {
        Route.rules.Add(new XrayRouteRuleModel());
    }
    private void OnDelCommand(XrayRouteRuleModel item)
    {
        Route.rules.Remove(item);
        OnSaveCommand();
        Init();
    }
}
