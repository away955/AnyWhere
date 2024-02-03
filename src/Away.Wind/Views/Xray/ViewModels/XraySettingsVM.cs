using Away.Service.Xray;

namespace Away.Wind.Views.Xray;

public class XraySettingsVM : BindableBase, INavigationAware
{
    private readonly ILogger<XraySettingsVM> _logger;
    private readonly IXrayService _xrayService;
    private readonly IDialogService _dialogService;

    public XraySettingsVM(
        ILogger<XraySettingsVM> logger,
        IXrayService xrayService,
        IDialogService dialogService
        )
    {
        _logger = logger;
        _xrayService = xrayService;
        _dialogService = dialogService;

        XrayConfig = _xrayService.GetConfig() ?? new XrayConfig();
        SaveXrayConfigCommand = new(OnSaveXrayConfigCommand);
        ShowDialogCommand = new(OnShowDialogCommand);

        ResetCommand();
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        ResetCommand();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        ResetCommand();
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    private void ResetCommand()
    {
        IsOpenXray = _xrayService.IsOpened;
    }


    private bool _isOpenXray;
    public bool IsOpenXray
    {
        get => _isOpenXray;
        set
        {
            SetProperty(ref _isOpenXray, value);
            if (_isOpenXray)
            {
                _xrayService.XrayStart();
            }
            else
            {
                _xrayService.XrayClose();
            }
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
