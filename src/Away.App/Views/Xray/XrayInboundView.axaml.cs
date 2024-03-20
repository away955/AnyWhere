namespace Away.App.Views;

[View("xray-setting-inbound")]
public partial class XrayInboundView : ReactiveUserControl<XrayInboundViewModel>, IView
{
    public XrayInboundView()
    {
        ViewModel = AwayLocator.GetViewModel<XrayInboundViewModel>();
        InitializeComponent();
    }
}