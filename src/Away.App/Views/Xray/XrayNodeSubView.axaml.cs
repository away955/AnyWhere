namespace Away.App.Views;

[View("xray-node-sub")]
public partial class XrayNodeSubView : UserControl, IView
{
    public XrayNodeSubView()
    {
        DataContext = AwayLocator.GetViewModel<XrayNodeSubViewModel>();
        InitializeComponent();
    }
}