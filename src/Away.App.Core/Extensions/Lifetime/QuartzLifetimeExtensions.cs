using Avalonia.Controls.ApplicationLifetimes;
using Away.App.Core.Extensions.DependencyInjection;
using Quartz;
using Serilog;

namespace Away.App.Core.Extensions.Lifetime;

public static class QuartzLifetimeExtensions
{
    private static Func<Task> Shutdown = null!;
    public static void AddQuartz(this IClassicDesktopStyleApplicationLifetime lifetime)
    {
        lifetime.Startup += Lifetime_Startup;
        lifetime.Exit += Lifetime_Exit;
    }

    private static async void Lifetime_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        await Shutdown.Invoke();
    }

    private static async void Lifetime_Startup(object? sender, ControlledApplicationLifetimeStartupEventArgs e)
    {
        if (sender is not IClassicDesktopStyleApplicationLifetime lifetime)
        {
            return;
        }
        Log.Information("启动定时任务服务...");
        var provider = lifetime.GetServiceProvider();
        var factory = provider.GetRequiredService<ISchedulerFactory>();
        IScheduler scheduler = await factory.GetScheduler();
        await scheduler.Start();
        Shutdown = () => scheduler.Shutdown();
    }
}
