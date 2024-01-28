using Away.Service.Xray;

namespace Away.Wind.ViewModels;

public class SystemSettingsViewModel : BindableBase, INavigationAware
{
    private readonly IXrayNodeService _xrayNodeService;

    public SystemSettingsViewModel(IXrayNodeService xrayNodeService)
    {
        _xrayNodeService = xrayNodeService;
        RunCommand = new DelegateCommand(OnRunCommand);
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {

    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {

    }


    public DelegateCommand RunCommand { get; set; }
    private void OnRunCommand()
    {
        _xrayNodeService.GetXrayNodeByUrl("https://bulinkbulink.com/freefq/free/master/v2");
    }
}
