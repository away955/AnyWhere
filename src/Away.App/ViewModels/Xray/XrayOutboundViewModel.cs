namespace Away.App.ViewModels;

[ViewModel]
public class XrayOutboundViewModel : ViewModelXrayBase
{
    [Reactive]
    public ObservableCollection<XrayOutboundModel> Items { get; set; } = [];

    public XrayOutboundViewModel(IXrayService xrayService, IMapper mapper) : base(xrayService, mapper)
    {
    }

    protected override void Init()
    {
        var items = _xrayService.Config.outbounds.Select(_mapper.Map<XrayOutboundModel>);
        Items = new ObservableCollection<XrayOutboundModel>(items);
    }
}
