using Avalonia.Controls.ApplicationLifetimes;
using Away.App.Views;

namespace Away.App;

public partial class App : Application
{
    public override void Initialize()
    {
        DataContext = AwayLocator.GetViewModel<AppViewModel>();
        AvaloniaXamlLoader.Load(this);
        MessageShutdown.Listen(args =>
        {
            if (ApplicationLifetime is IControlledApplicationLifetime control)
            {
                control.Shutdown();
            }            
        });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        base.OnFrameworkInitializationCompleted();
    }

}