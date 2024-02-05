namespace Away.Wind.Views.Xray.ViewModels;

public class XraySettingsVM : BindableBase
{
    private readonly IRegionManager _regionManager;

    public XraySettingsVM(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        NavCommand = new(OnNavCommand);
    }

    public DelegateCommand<SelectionChangedEventArgs?> NavCommand { get; private set; }
    private void OnNavCommand(SelectionChangedEventArgs? e)
    {
        string? s;
        if (e == null || e.AddedItems[0] is not TabItem t)
        {
            s = "xray-log-settings";
        }
        else
        {
            s = Convert.ToString(t.Tag);
        }
        _regionManager.RequestNavigate("XraySettingsBox", s);
    }
}
