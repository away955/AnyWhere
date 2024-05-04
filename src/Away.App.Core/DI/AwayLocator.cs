using Away.App.Core.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace Away.App.Core.DI;

public static class AwayLocator
{
    public static ServiceCollection Services { get; private set; }

    private static IServiceProvider _serviceProvider = null!;
    public static IServiceProvider ServiceProvider => _serviceProvider ??= Services.BuildServiceProvider();

    static AwayLocator()
    {
        Services = new();
    }

    public static IView? GetView(string? url)
    {
        return ServiceProvider.GetKeyedService<IView>(url);
    }
    public static T GetViewModel<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    public static T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    public static IEnumerable<T> GetKeyedServices<T>(string key) where T : notnull
    {
        return ServiceProvider.GetKeyedServices<T>(key);
    }
}
