namespace Away.Wind.ViewModels;

public class XrayRouteSettingsVM : SettingsVMBase
{
    public XrayRouteSettingsVM(IXrayService xrayService, IMapper mapper, IMessageService messageService)
        : base(xrayService, mapper, messageService)
    {
        AddCommand = new(OnAddCommand);
        DelCommand = new(OnDelCommand);
    }

    protected override void Init()
    {
        Route = _mapper.Map<XrayRouteModel>(_xrayService.Config.routing);
    }

    protected override void OnSaveCommand()
    {
        _xrayService.Config.routing = _mapper.Map<XrayRoute>(Route);
        base.OnSaveCommand();
    }

    private XrayRouteModel _route = null!;
    public XrayRouteModel Route
    {
        get => _route;
        set => SetProperty(ref _route, value);
    }

    public DelegateCommand AddCommand { get; private set; }
    public DelegateCommand<XrayRouteRuleModel?> DelCommand { get; private set; }
    private void OnAddCommand()
    {
        Route.rules.Add(new XrayRouteRuleModel());
    }
    private void OnDelCommand(XrayRouteRuleModel? item)
    {
        Route.rules.Remove(item!);
    }
}
