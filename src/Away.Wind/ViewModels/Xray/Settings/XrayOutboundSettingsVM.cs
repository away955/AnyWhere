
namespace Away.Wind.ViewModels;

public class XrayOutboundSettingsVM : SettingsVMBase
{
    public XrayOutboundSettingsVM(IXrayService xrayService, IMapper mapper, IMessageService messageService) 
        : base(xrayService, mapper, messageService)
    {
    }

    private ObservableCollection<XrayOutbound> _items = [];

  

    public ObservableCollection<XrayOutbound> Items { get => _items; set => SetProperty(ref _items, value); }

    protected override void Init()
    {
        Items = new ObservableCollection<XrayOutbound>(_xrayService.Config.outbounds);
    } 

    protected override void OnSaveCommand()
    {

    }
}
