using Avalonia.Controls.ApplicationLifetimes;
using Away.App.Core.Windows.ProcessOnly;
using Away.App.Views;

namespace Away.App;

public partial class App : Application
{

    public override void Initialize()
    {
        var processOnly = AwayLocator.GetService<IProcessOnly>();
        if (processOnly.Show("Away.AnyWhere"))
        {
            Environment.Exit(0);
            return;
        }

        DataContext = AwayLocator.GetViewModel<AppViewModel>();
        AvaloniaXamlLoader.Load(this);
        MessageBus.Current.Subscribe(MessageBusType.Shutdown, args =>
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