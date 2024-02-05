﻿namespace Away.Wind.Views.Xray.ViewModels;

public abstract class SettingsVMBase : BindableBase, INavigationAware
{
    protected readonly IXrayService _xrayService;
    protected readonly IMapper _mapper;

    public SettingsVMBase(IXrayService xrayService, IMapper mapper)
    {
        _xrayService = xrayService;
        _mapper = mapper;

        SaveCommand = new(OnSaveCommand);
        CancelCommand = new(OnCancelCommand);
    }


    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        Init();
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        Init();
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
    }

    protected abstract void Init();
    public DelegateCommand SaveCommand { get; private set; }
    protected abstract void OnSaveCommand();

    public DelegateCommand CancelCommand { get; private set; }
    protected abstract void OnCancelCommand();

}
