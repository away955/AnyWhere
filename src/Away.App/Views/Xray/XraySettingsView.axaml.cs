namespace Away.App.Views;

[View("xray-settings")]
public partial class XraySettingsView : UserControl, IView
{
    private readonly TabControl _tabControl;
    public XraySettingsView()
    {
        InitializeComponent();
        _tabControl = this.FindControl<TabControl>("TabSettings")!;
        _tabControl.SelectionChanged += TabControl_SelectionChanged;
        _tabControl.Initialized += TabControl_Initialized;
    }

    private void TabControl_Initialized(object? sender, EventArgs e)
    {
        if (_tabControl.SelectedItem is not TabItem tabItem)
        {
            return;
        }
        ViewChange(tabItem);
    }

    private void TabControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0 || e.AddedItems[0] is not TabItem tabItem)
        {
            return;
        }
        ViewChange(tabItem);
    }

    private static void ViewChange(TabItem tabItem)
    {
        var url = tabItem.Tag as string;
        var view = AwayLocator.GetView(url) ?? AwayLocator.GetView("404");
        tabItem.Content = view;
    }
}