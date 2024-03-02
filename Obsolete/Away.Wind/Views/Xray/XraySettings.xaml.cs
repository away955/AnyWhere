namespace Away.Wind.Views;


[Dialog("xray-settings")]
[Navigation("xray-settings")]
[ViewModel(typeof(XraySettingsVM))]
public partial class XraySettings : UserControl
{
    public XraySettings()
    {
        InitializeComponent();
    }
}
