namespace Away.App.Views;

[View("xray-setting-route")]
public partial class XrayRouteView : UserControl, IView
{
    public XrayRouteView()
    {
        DataContext = AwayLocator.GetViewModel<XrayRouteViewModel>();
        InitializeComponent();
    }
}