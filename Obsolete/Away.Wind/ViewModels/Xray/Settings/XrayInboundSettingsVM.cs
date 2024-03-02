using Away.Service.XrayNode;

namespace Away.Wind.ViewModels;

public class XrayInboundSettingsVM : SettingsVMBase
{
    public XrayInboundSettingsVM(IXrayService xrayService, IMapper mapper, IMessageService messageService)
        : base(xrayService, mapper, messageService)
    {
        AddCommand = new(OnAddCommand);
        DelCommand = new(OnDelCommand);
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

    private ObservableCollection<XrayInboundModel> _items = [];
    public ObservableCollection<XrayInboundModel> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public DelegateCommand AddCommand { get; private set; }
    public DelegateCommand<XrayInboundModel?> DelCommand { get; private set; }

    private void OnAddCommand()
    {
        Items.Add(new XrayInboundModel());
    }
    private void OnDelCommand(XrayInboundModel? model)
    {
        if (model == null)
        {
            return;
        }
        Items.Remove(model);
        _xrayService.Config.RemoveInbound(model.tag);
    }
}
