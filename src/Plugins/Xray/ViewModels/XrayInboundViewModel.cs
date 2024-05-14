namespace Xray.ViewModels;

public sealed class XrayInboundViewModel : ViewModelXrayBase
{

    [Reactive]
    public ObservableCollection<XrayInboundModel> Items { get; set; } = [];
    public static List<string> ProtocolItems => ["http", "socks"];

    public ICommand AddCommand { get; private set; }
    public ICommand DelCommand { get; private set; }

    public XrayInboundViewModel(IXrayService xrayService, IXrayMapper mapper) : base(xrayService, mapper)
    {
        AddCommand = ReactiveCommand.Create(OnAddCommand);
        DelCommand = ReactiveCommand.Create<XrayInboundModel>(OnDelCommand);
    }

    protected override void Init()
    {
        var items = _xrayService.Config.inbounds.Select(_mapper.Map<XrayInboundModel>);
        Items = new ObservableCollection<XrayInboundModel>(items);
    }

    protected override void OnSaveCommand()
    {
        foreach (var item in Items)
        {
            var inbound = _mapper.Map<XrayInbound>(item);
            _xrayService.Config.SetInbound(inbound);
        }
        base.OnSaveCommand();
    }

    private void OnAddCommand()
    {
        Items.Add(new XrayInboundModel());
    }
    private void OnDelCommand(XrayInboundModel model)
    {
        Items.Remove(model);
        _xrayService.Config.RemoveInbound(model.tag);
        OnSaveCommand();
    }
}
