namespace Xray.ViewModels;

public sealed class XrayDnsViewModel : ViewModelXrayBase
{
    [Reactive]
    public XrayDnsModel Dns { get; set; } = new();
    public ICommand AddCommand { get; }
    public ICommand DelCommand { get; }

    public XrayDnsViewModel(IXrayService xrayService, IMapper mapper) : base(xrayService, mapper)
    {
        AddCommand = ReactiveCommand.Create(OnAddCommand);
        DelCommand = ReactiveCommand.Create<StringItem>(OnDelCommand);
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


    private void OnAddCommand()
    {
        Dns.Items.Add(new StringItem());
    }
    private void OnDelCommand(StringItem item)
    {
        Dns.Items.Remove(item);
    }
}
