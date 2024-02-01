using Away.Wind.Views.Xray;

namespace Away.Wind.Views;


[Navigation("xray-nodes")]
[ViewModel(typeof(XrayNodesViewModel))]
public partial class XrayNodes : UserControl
{
    public XrayNodes()
    {
        InitializeComponent();
    }
}
