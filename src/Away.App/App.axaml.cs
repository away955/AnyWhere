using Avalonia.Controls.ApplicationLifetimes;
using Away.App.Views;

namespace Away.App;

public partial class App : Application
{
    public IServiceProvider Services => this.GetServiceProvider();
    public new static App? Current => Application.Current as App;

    public override void Initialize()
    {       
        AvaloniaXamlLoader.Load(this);
        DataContext = Services.GetViewModel<AppViewModel>();
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