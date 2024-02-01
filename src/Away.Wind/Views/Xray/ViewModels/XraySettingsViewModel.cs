using Away.Service.Xray;

namespace Away.Wind.Views.Xray;

public class XraySettingsViewModel : BindableBase
{
    private readonly ILogger<XraySettingsViewModel> _logger;
    private readonly IXrayService _xrayService;

    private readonly IDialogService _dialogService;

    public XraySettingsViewModel(
        ILogger<XraySettingsViewModel> logger,
        IXrayService xrayService,
        IDialogService dialogService
        )
    {
        _logger = logger;
        _xrayService = xrayService;
        _dialogService = dialogService;

        XrayConfig = _xrayService.GetConfig() ?? new XrayConfig();
        SaveXrayConfigCommand = new DelegateCommand(OnSaveXrayConfigCommand);
        ShowDialogCommand = new DelegateCommand<string>(OnShowDialogCommand);

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


    public DelegateCommand<string> ShowDialogCommand { get; set; }
    private void OnShowDialogCommand(string message)
    {
        _dialogService.Show(message, new DialogParameters(), null);
    }

}
