namespace Xray.Views;

public partial class XrayTestSettingsView : ReactiveUserControl<XrayTestSettingsViewModel>, IView
{
    public XrayTestSettingsView()
    {
        ViewModel = AwayLocator.GetService<XrayTestSettingsViewModel>();
        InitializeComponent();
    }
}