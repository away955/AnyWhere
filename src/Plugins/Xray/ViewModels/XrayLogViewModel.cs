namespace Xray.ViewModels;

public class XrayLogViewModel : ViewModelXrayBase
{
    [Reactive]
    public XrayLogModel Log { get; set; } = new();
    public static List<string> LogLevelItems => ["none", "error", "warning", "info", "debug"];

    public XrayLogViewModel(IXrayService xrayService, IMapper mapper) : base(xrayService, mapper)
    {
    }

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
