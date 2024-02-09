namespace Away.Wind.ViewModels;

public class XrayOutboundSettingsVM : SettingsVMBase
{
    public XrayOutboundSettingsVM(IXrayService xrayService, IMapper mapper) : base(xrayService, mapper)
    {
    }

    private ObservableCollection<XrayOutbound> _outbound = [];
    public ObservableCollection<XrayOutbound> Outbound { get => _outbound; set => SetProperty(ref _outbound, value); }

    protected override void Init()
    {
        Outbound = new ObservableCollection<XrayOutbound>(_xrayService.Config.outbounds);
    }

    protected override void OnCancelCommand()
    {
    }

    protected override void OnSaveCommand()
    {

    }
}
