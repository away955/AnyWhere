namespace Away.App.Views;

public partial class LeftMenu : UserControl
{
    private readonly LeftMenuViewModel _vm;
    public LeftMenu()
    {
        _vm = AwayLocator.GetService<LeftMenuViewModel>()!;
        this.DataContext = _vm;
        InitializeComponent();

        Menu_Left.Tapped += Menu_Tapped;
        InitDefaultMenu();
    }

    private void InitDefaultMenu()
    {
        var url = _vm!.DefaultMenu;
        foreach (var item in Menu_Left.Items.Cast<MenuItem>())
        {
            if (item.Name == "logo")
            {
                continue;
            }
            var constUrl = Convert.ToString(item.CommandParameter);
            ChangeMenuHeaderForeground(item, constUrl == url);
        }
        MessageRouter.Go(_vm!.DefaultMenu);
    }

    private void Menu_Tapped(object? sender, TappedEventArgs e)
    {
        foreach (var item in Menu_Left.Items.Cast<MenuItem>())
        {
            if (item.Name == "logo")
            {
                if (item.IsSelected)
                {
                    break;
                }
                continue;
            }
            ChangeMenuHeaderForeground(item, item.IsSelected);
        }
    }

    private static void ChangeMenuHeaderForeground(MenuItem item, bool flag)
    {
        item.Foreground = flag ? Brushes.BlueViolet : Brushes.HotPink;
    }

}