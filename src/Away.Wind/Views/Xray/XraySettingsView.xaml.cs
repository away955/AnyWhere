using Away.Wind.Views.Xray;

namespace Away.Wind.Views;


[Navigation("xray-settings")]
[ViewModel(typeof(XraySettingsViewModel))]
public partial class XraySettingsView : UserControl
{
    public XraySettingsView()
    {
        InitializeComponent();
    }
}
