namespace Away.App.Views;

public partial class LeftMenu : ReactiveUserControl<LeftMenuViewModel>
{
    public LeftMenu()
    {
        ViewModel = AwayLocator.GetService<LeftMenuViewModel>();
        InitializeComponent();
    }

    public void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0)
        {
            return;
        }
        if (e.AddedItems[0] is not AppMenuModel model)
        {
            return;
        }
        MessageRouter.Go(model.Path);
    }
}