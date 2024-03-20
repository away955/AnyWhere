namespace Away.App.Views;

[View("xray-node-sub")]
public partial class XrayNodeSubView : ReactiveUserControl<XrayNodeSubViewModel>, IView
{
    public XrayNodeSubView()
    {
        ViewModel = AwayLocator.GetViewModel<XrayNodeSubViewModel>();
        InitializeComponent();
    }
}