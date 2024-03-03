namespace Away.App.Views;

[View("xray-setting-inbound")]
public partial class XrayInboundView : UserControl, IView
{
    public XrayInboundView()
    {
        DataContext = AwayLocator.GetViewModel<XrayInboundViewModel>();
        InitializeComponent();
    }
}