using Away.Wind.Views.Systems;

namespace Away.Wind.Views;

[Navigation("system-settings")]
[ViewModel(typeof(SystemSettingsViewModel))]
public partial class SystemSettings : UserControl
{
    public SystemSettings()
    {
        InitializeComponent();
    }
}
