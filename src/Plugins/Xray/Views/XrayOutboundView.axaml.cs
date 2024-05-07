namespace Xray.Views;

public partial class XrayOutboundView : ReactiveUserControl<XrayOutboundViewModel>, IView
{
    public XrayOutboundView()
    {
        ViewModel = AwayLocator.GetService<XrayOutboundViewModel>();
        InitializeComponent();
    }
}