namespace Away.Wind.ViewModels;

public class XrayApiSettingsVM : SettingsVMBase
{
    public XrayApiSettingsVM(IXrayService xrayService, IMapper mapper) : base(xrayService, mapper)
    {
    }

    protected override void Init()
    {

    }

    protected override void OnCancelCommand()
    {

    }

    protected override void OnSaveCommand()
    {

    }
    public ObservableCollection<string> XrayApiItems { get; set; } =
    [
        "HandlerService",
        "LoggerService",
        "StatsService"
    ];
}
