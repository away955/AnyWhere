namespace Away.Wind.ViewModels;

public class XraySettingsVM : BindableBase, IDialogAware
{
    private readonly IXrayNodeSubRepository _nodeSubRepository;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    public XraySettingsVM(
        IXrayNodeSubRepository nodeSubRepository,
        IMapper mapper,
        IMessageService messageService
        )
    {
        _nodeSubRepository = nodeSubRepository;
        _mapper = mapper;
        _messageService = messageService;

        Init();
    }

    private XrayNodeSubSettingsVM _nodeSubSettingsVM = null!;
    public XrayNodeSubSettingsVM NodeSubSettingsVM
    {
        get => _nodeSubSettingsVM;
        set => SetProperty(ref _nodeSubSettingsVM, value);
    }

    private XrayLogSettingsVM _logSettingsVM = null!;
    public XrayLogSettingsVM LogSettingsVM
    {
        get => _logSettingsVM;
        set => SetProperty(ref _logSettingsVM, value);
    }

    private XrayApiSettingsVM _apiSettingsVM = null!;
    public XrayApiSettingsVM ApiSettingsVM
    {
        get => _apiSettingsVM;
        set => SetProperty(ref _apiSettingsVM, value);
    }

    private XrayDnsSettingsVM _dnsSettingsVM = null!;
    public XrayDnsSettingsVM DNSSettingsVM
    {
        get => _dnsSettingsVM;
        set => SetProperty(ref _dnsSettingsVM, value);
    }

    private XrayInboundSettingsVM _inboundSettingsVM = null!;
    public XrayInboundSettingsVM InboundSettingsVM
    {
        get => _inboundSettingsVM;
        set => SetProperty(ref _inboundSettingsVM, value);
    }

    private XrayOutboundSettingsVM _outboundSettingsVM = null!;

    public XrayOutboundSettingsVM OutboundSettingsVM
    {
        get => _outboundSettingsVM;
        set => SetProperty(ref _outboundSettingsVM, value);
    }


    private void Init()
    {
        NodeSubSettingsVM = new(_nodeSubRepository, _mapper, _messageService);
    }


    public string Title => "网络配置";
    public event Action<IDialogResult> RequestClose = null!;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        Init();
    }
}
