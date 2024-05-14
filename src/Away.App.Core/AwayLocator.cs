namespace Away.App.Core;

public static class AwayLocator
{
    static AwayLocator()
    {
        Services = new();
    }

    public static ServiceCollection Services { get; private set; }
    private static ServiceProvider _serviceProvider = null!;
    public static IServiceProvider ServiceProvider => _serviceProvider ??= Services.BuildServiceProvider();

    public static T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    public static IEnumerable<T> GetServices<T>(string key) where T : notnull
    {
        return ServiceProvider.GetKeyedServices<T>(key);
    }

    public static void Refresh()
    {
        _serviceProvider.Dispose();
        _serviceProvider = Services.BuildServiceProvider();
    }
}
