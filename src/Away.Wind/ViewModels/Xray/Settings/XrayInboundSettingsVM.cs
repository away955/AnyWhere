namespace Away.Wind.ViewModels;

public class XrayInboundSettingsVM : SettingsVMBase
{
    public XrayInboundSettingsVM(IXrayService xrayService, IMapper mapper) : base(xrayService, mapper)
    {
    }

    protected override void Init()
    {
        var items = _xrayService.Config.inbounds.Select(_mapper.Map<XrayInboundModel>).ToList();
        Inbound = new ObservableCollection<XrayInboundModel>(items);
    }

    protected override void OnCancelCommand()
    {
    }

    protected override void OnSaveCommand()
    {
    }

    private ObservableCollection<XrayInboundModel> _inbound = [];
    public ObservableCollection<XrayInboundModel> Inbound
    {
        get => _inbound;
        set => SetProperty(ref _inbound, value);
    }
}
