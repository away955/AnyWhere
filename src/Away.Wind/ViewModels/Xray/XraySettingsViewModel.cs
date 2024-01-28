using Away.Service.Xray;
using Away.Service.Xray.Model;

namespace Away.Wind.ViewModels;

public class XraySettingsViewModel : BindableBase
{
    private readonly ILogger<XraySettingsViewModel> _logger;
    private readonly IXrayService _xrayService;

    public XraySettingsViewModel(
        ILogger<XraySettingsViewModel> logger,
        IXrayService xrayService
        )
    {
        _logger = logger;
        _xrayService = xrayService;


        XrayConfig = _xrayService.GetConfig() ?? new XrayConfig();
        SaveXrayConfigCommand = new DelegateCommand(OnSaveXrayConfigCommand);
        RunCommand = new DelegateCommand(OnRunCommand);
    }

    private XrayConfig? _xrayConfig;
    public XrayConfig? XrayConfig
    {
        get => _xrayConfig;
        set => SetProperty(ref _xrayConfig, value);
    }

    public DelegateCommand SaveXrayConfigCommand { get; set; }
    public void OnSaveXrayConfigCommand()
    {
        _xrayService.SetConfig(_xrayConfig!);
    }

    public DelegateCommand RunCommand { get; private set; }
    private void OnRunCommand()
    {
        _xrayService.Run();
    }

    public ObservableCollection<MultiComboBoxModel> XrayApiItems { get; set; } =
    [
        new MultiComboBoxModel("HandlerService"),
        new MultiComboBoxModel("LoggerService"),
        new MultiComboBoxModel("StatsService")
    ];


}
