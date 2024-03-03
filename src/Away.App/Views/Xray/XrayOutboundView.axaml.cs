namespace Away.App.Views;

[View("xray-setting-outbound")]
public partial class XrayOutboundView : UserControl, IView
{
    public XrayOutboundView()
    {
        DataContext = AwayLocator.GetViewModel<XrayOutboundViewModel>();
        InitializeComponent();
    }
}