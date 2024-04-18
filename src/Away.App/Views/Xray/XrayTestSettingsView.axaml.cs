namespace Away.App.Views;

[View("xray-test-settings")]
public partial class XrayTestSettingsView : ReactiveUserControl<XrayTestSettingsViewModel>, IView
{
    public XrayTestSettingsView()
    {
        ViewModel = AwayLocator.GetViewModel<XrayTestSettingsViewModel>();
        InitializeComponent();
    }
}