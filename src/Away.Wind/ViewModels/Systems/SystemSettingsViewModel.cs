namespace Away.Wind.ViewModels;

public class SystemSettingsViewModel : BindableBase, INavigationAware
{
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
}
