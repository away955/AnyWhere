namespace Away.App.Views;

[View("xray-node")]
public partial class XrayNodesView : ReactiveUserControl<XrayNodesViewModel>, IView
{
    public XrayNodesView()
    {
        ViewModel = AwayLocator.GetViewModel<XrayNodesViewModel>();
        InitializeComponent();

        DGXrayNode.DoubleTapped += DataGrid_DoubleTapped;
    }

    /// <summary>
    /// Ë«»÷Ñ¡ÖÐ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGrid_DoubleTapped(object? sender, TappedEventArgs e)
    {
        MessageBus.Current.Publish(MessageBusType.Event, e, "DGXrayNode");
    }
}