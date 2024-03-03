namespace Away.App.Views;

public partial class LeftMenu : UserControl
{
    private readonly Menu _menu;
    private readonly LeftMenuViewModel _vm;
    public LeftMenu()
    {
        _vm = AwayLocator.GetViewModel<LeftMenuViewModel>()!;
        this.DataContext = _vm;
        InitializeComponent();
        _menu = this.FindControl<Menu>("Menu_Left")!;
        _menu.Tapped += Menu_Tapped;
        InitDefaultMenu();
    }

    private void InitDefaultMenu()
    {
        var url = _vm!.DefaultMenu;
        foreach (var item in _menu.Items.Cast<MenuItem>())
        {
            if (item.Name == "logo")
            {
                continue;
            }
            var constUrl = Convert.ToString(item.CommandParameter);
            item.Foreground = constUrl == url ? Brush.Parse("#0532ff") : Brush.Parse("#fff");
        }
        Dispatcher.UIThread.Post(() =>
        {
            MessageBus.Current.Publish(MessageBusType.NavMainBox, _vm!.DefaultMenu);
        });
    }

    private void Menu_Tapped(object? sender, TappedEventArgs e)
    {
        foreach (var item in _menu.Items.Cast<MenuItem>())
        {
            if (item.Name == "logo")
            {
                if (item.IsSelected)
                {
                    break;
                }
                continue;
            }
            item.Foreground = item.IsSelected ? Brush.Parse("#0532ff") : Brush.Parse("#fff");
        }
    }

}