using Avalonia.Controls.ApplicationLifetimes;
using Away.App.Update.Views;

namespace Away.App.Update;

public partial class App : Application
{
    public static ServiceCollection Services { get; private set; }
    private static IServiceProvider _serviceProvider = null!;
    public static IServiceProvider ServiceProvider => _serviceProvider ??= Services.BuildServiceProvider();
    static App()
    {
        Services = new();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
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