namespace Xray.Views;

public partial class XrayInboundView : ReactiveUserControl<XrayInboundViewModel>, IView
{
    public XrayInboundView()
    {
        ViewModel = AwayLocator.GetService<XrayInboundViewModel>();
        InitializeComponent();
    }
}