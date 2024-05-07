namespace Xray.Views;

public partial class XrayRouteView : ReactiveUserControl<XrayRouteViewModel>, IView
{
    public XrayRouteView()
    {
        ViewModel = AwayLocator.GetService<XrayRouteViewModel>();
        InitializeComponent();
    }
}