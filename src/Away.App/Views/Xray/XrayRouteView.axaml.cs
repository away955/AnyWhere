namespace Away.App.Views;

[View("xray-setting-route")]
public partial class XrayRouteView : ReactiveUserControl<XrayRouteViewModel>, IView
{
    public XrayRouteView()
    {
        ViewModel = AwayLocator.GetViewModel<XrayRouteViewModel>();
        InitializeComponent();
    }
}