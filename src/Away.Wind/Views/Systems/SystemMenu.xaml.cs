using Away.Wind.Views.Systems;

namespace Away.Wind.Views;


[Navigation("system-menu")]
[ViewModel(typeof(SystemMenuViewModel))]
public partial class SystemMenu : UserControl
{
    public SystemMenu()
    {
        InitializeComponent();
    }        
}
