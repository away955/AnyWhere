using Avalonia.Input;

namespace Xray.Views;

public partial class XrayNodesView : ReactiveUserControl<XrayNodesViewModel>, IView
{
    public XrayNodesView()
    {
        ViewModel = AwayLocator.GetService<XrayNodesViewModel>();
        InitializeComponent();

        LB_Nodes.DoubleTapped += DataGrid_DoubleTapped;
    }

    /// <summary>
    /// ˫��ѡ��
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGrid_DoubleTapped(object? sender, TappedEventArgs e)
    {
        MessageEvent.Run(e, XrayNodesViewModel.CheckedEvent);
    }
}

