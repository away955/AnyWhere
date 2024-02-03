using Away.Wind.Views.Xray;

namespace Away.Wind.Views;


[Navigation("xray-settings")]
[ViewModel(typeof(XraySettingsVM))]
public partial class XraySettings : UserControl
{
    public XraySettings()
    {
        InitializeComponent();
    }
}
