
namespace Away.Wind.ViewModels;

public class XrayLogSettingsVM : SettingsVMBase
{
    public XrayLogSettingsVM(IXrayService xrayService, IMapper mapper, IMessageService messageService)
        : base(xrayService, mapper, messageService)
    {
    }

    private XrayLogModel _log = new();

    public XrayLogModel Log { get => _log; set => SetProperty(ref _log, value); }

    protected override void Init()
    {
        var log = _xrayService.Config.log ?? new XrayLog();
        Log = _mapper.Map<XrayLogModel>(log);
    }

    protected override void OnSaveCommand()
    {
        _xrayService.Config.log = _mapper.Map<XrayLog>(Log);
        base.OnSaveCommand();
    }
}
