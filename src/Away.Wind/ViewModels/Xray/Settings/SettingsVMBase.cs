namespace Away.Wind.ViewModels;

public abstract class SettingsVMBase : BindableBase
{
    protected readonly IXrayService _xrayService;
    protected readonly IMapper _mapper;

    public SettingsVMBase(IXrayService xrayService, IMapper mapper)
    {
        _xrayService = xrayService;
        _mapper = mapper;

        SaveCommand = new(OnSaveCommand);
        CancelCommand = new(OnCancelCommand);
        Init();
    }

    protected abstract void Init();
    public DelegateCommand SaveCommand { get; private set; }
    protected abstract void OnSaveCommand();

    public DelegateCommand CancelCommand { get; private set; }
    protected abstract void OnCancelCommand();

}
