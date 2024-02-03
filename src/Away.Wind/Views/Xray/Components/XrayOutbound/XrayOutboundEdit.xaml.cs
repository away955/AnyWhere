using Away.Wind.Views.Xray;

namespace Away.Wind.Views;


[Dialog("xray-outbound")]
[ViewModel(typeof(XrayOutboundEditVM))]
public partial class XrayOutboundEdit : UserControl
{
    public XrayOutboundEdit()
    {
        InitializeComponent();
    }
}
