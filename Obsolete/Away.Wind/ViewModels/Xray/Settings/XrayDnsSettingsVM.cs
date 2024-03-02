namespace Away.Wind.ViewModels;

public class XrayDnsSettingsVM : SettingsVMBase
{
    public XrayDnsSettingsVM(IXrayService xrayService, IMapper mapper, IMessageService messageService)
        : base(xrayService, mapper, messageService)
    {
        AddCommand = new(OnAddCommand);
        DelCommand = new(OnDelCommand);
    }

    protected override void Init()
    {
        Dns = _mapper.Map<XrayDnsModel>(_xrayService.Config.dns);
    }


    protected override void OnSaveCommand()
    {
        _xrayService.Config.dns = _mapper.Map<XrayDns>(Dns);
        base.OnSaveCommand();
    }

    private XrayDnsModel _dns = new();
    public XrayDnsModel Dns
    {
        get => _dns;
        set => SetProperty(ref _dns, value);
    }

    public DelegateCommand AddCommand { get; private set; }
    public DelegateCommand<StringItem?> DelCommand { get; private set; }
    private void OnAddCommand()
    {
        Dns.Items.Add(new StringItem());
    }
    private void OnDelCommand(StringItem? item)
    {
        Dns.Items.Remove(item!);
    }
}
