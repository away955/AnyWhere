using Away.Wind.Views.Systems;

namespace Away.Wind.Views;

[Dialog("system-proxy")]
[ViewModel(typeof(SystemProxyViewModel))]
public partial class SystemProxy : UserControl
{
    public SystemProxy()
    {
        InitializeComponent();
    }
}
