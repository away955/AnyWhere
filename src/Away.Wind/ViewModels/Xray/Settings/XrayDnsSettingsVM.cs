namespace Away.Wind.ViewModels;

public class XrayDnsSettingsVM : SettingsVMBase
{
    public XrayDnsSettingsVM(IXrayService xrayService, IMapper mapper) : base(xrayService, mapper)
    {
    }

    protected override void Init()
    {
        Dns = _xrayService.Config.dns;
    }

    protected override void OnCancelCommand()
    {

    }

    protected override void OnSaveCommand()
    {

    }

    private XrayDns _dns = null!;
    public XrayDns Dns
    {
        get => _dns;
        set => SetProperty(ref _dns, value);
    }
}
