namespace Away.Wind.ViewModels;

public abstract class SettingsVMBase : BindableBase
{
    protected readonly IMessageService _messageService;
    protected readonly IXrayService _xrayService;
    protected readonly IMapper _mapper;

    public SettingsVMBase(
        IXrayService xrayService,
        IMapper mapper,
        IMessageService messageService)
    {
        _xrayService = xrayService;
        _mapper = mapper;
        _messageService = messageService;

        SaveCommand = new(OnSaveCommand);
        CancelCommand = new(OnCancelCommand);
        Init();
        _messageService = messageService;
    }


    protected abstract void Init();
    public DelegateCommand SaveCommand { get; private set; }
    protected virtual void OnSaveCommand()
    {
        _xrayService.SaveConfig();
        _messageService.Show("保存成功");
    }

    public DelegateCommand CancelCommand { get; private set; }
    protected void OnCancelCommand()
    {
        Init();
    }

}
