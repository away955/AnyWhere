namespace Xray.Views;

public partial class XrayNodeSubView : ReactiveUserControl<XrayNodeSubViewModel>, IView
{
    public XrayNodeSubView()
    {
        ViewModel = AwayLocator.GetService<XrayNodeSubViewModel>();
        InitializeComponent();
    }
}