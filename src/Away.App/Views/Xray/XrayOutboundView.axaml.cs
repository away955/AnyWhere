namespace Away.App.Views;

[View("xray-setting-outbound")]
public partial class XrayOutboundView : ReactiveUserControl<XrayOutboundViewModel>, IView
{
    public XrayOutboundView()
    {
        ViewModel = AwayLocator.GetViewModel<XrayOutboundViewModel>();
        InitializeComponent();
    }
}