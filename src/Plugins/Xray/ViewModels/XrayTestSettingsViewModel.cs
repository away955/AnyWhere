namespace Xray.ViewModels;

public sealed class XrayTestSettingsViewModel : ViewModelBase
{
    [Reactive]
    public XrayTestSettingsModel Item { get; set; } = new();

    public ICommand SaveCommand { get; }

    private readonly IMapper _mapper;
    private readonly IXraySettingService _xraySettingService;
    public XrayTestSettingsViewModel(IMapper mapper, IXraySettingService xraySettingService)
    {
        _mapper = mapper;
        _xraySettingService = xraySettingService;
        SaveCommand = ReactiveCommand.Create(OnSaveCommand);
        Init();
    }

    private void Init()
    {
        var settings = _xraySettingService.Get();
        Item = _mapper.Map<XrayTestSettingsModel>(settings);
    }

    protected override void OnActivation()
    {
        Init();
    }

    private void OnSaveCommand()
    {
        var settings = _mapper.Map<SpeedTestSettings>(Item);
        _xraySettingService.Set(settings);
        MessageShow.Success("保存成功");
    }
}
