namespace Away.App.Core;

public static class AwayLocator
{
    static AwayLocator()
    {
        Services = new();
    }

    public static ServiceCollection Services { get; private set; }
    private static IServiceProvider _serviceProvider = null!;
    public static IServiceProvider ServiceProvider => _serviceProvider ??= Services.BuildServiceProvider();

    public static T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    public static IEnumerable<T> GetServices<T>(string key) where T : notnull
    {
        return ServiceProvider.GetKeyedServices<T>(key);
    }
}
