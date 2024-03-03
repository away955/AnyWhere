namespace Away.App.ViewModels;

public abstract class ViewModelXrayBase : ViewModelBase
{
    protected readonly IXrayService _xrayService;
    protected readonly IMapper _mapper;


    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public ViewModelXrayBase(IXrayService xrayService, IMapper mapper)
    {
        _xrayService = xrayService;
        _mapper = mapper;

        SaveCommand = ReactiveCommand.Create(OnSaveCommand);
        CancelCommand = ReactiveCommand.Create(OnCancelCommand);
        Init();
    }


    protected abstract void Init();
    protected virtual void OnSaveCommand()
    {
        _xrayService.SaveConfig();
        Show("保存成功");
    }

    protected void OnCancelCommand()
    {
        _xrayService.GetConfig();
        Init();
    }
}
