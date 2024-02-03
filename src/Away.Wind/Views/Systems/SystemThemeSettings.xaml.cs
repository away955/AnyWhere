using Away.Wind.Views.Systems.ViewModels;

namespace Away.Wind.Views.Systems;

[Dialog("system-theme-settings")]
[ViewModel(typeof(SystemThemeSettingsVM))]
public partial class SystemThemeSettings : UserControl
{
    public SystemThemeSettings()
    {
        InitializeComponent();
    }
}
