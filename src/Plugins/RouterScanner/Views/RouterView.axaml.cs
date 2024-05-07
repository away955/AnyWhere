using Avalonia.ReactiveUI;
using Away.App;
using RouterScanner.ViewModels;

namespace RouterScanner.Views;

public partial class RouterView : ReactiveUserControl<RouterViewModel>
{
    public RouterView()
    {
        ViewModel = AwayLocator.GetService<RouterViewModel>();
        InitializeComponent();
    }
}