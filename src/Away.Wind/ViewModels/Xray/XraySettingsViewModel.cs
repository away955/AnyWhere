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

    }


    private bool _xrayIsEnable;
    public bool XrayIsEnable
    {
        get => _xrayIsEnable;
        set
        {
            if (_xrayIsEnable == false)
            {
                _xrayService.XrayStart();
            }
            else
            {
                _xrayService.XrayClose();
            }
            SetProperty(ref _xrayIsEnable, value);
        }
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



    public ObservableCollection<MultiComboBoxModel> XrayApiItems { get; set; } =
    [
        new MultiComboBoxModel("HandlerService"),
        new MultiComboBoxModel("LoggerService"),
        new MultiComboBoxModel("StatsService")
    ];


}
