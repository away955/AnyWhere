using Avalonia.Controls.ApplicationLifetimes;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class AppBuilderServiceExtensions
{
    private static IServiceProvider ServiceProvider { get; set; } = null!;
    public static AppBuilder UserContaierFactory(this AppBuilder builder, Action<IServiceCollection> configureServices)
    {
        var services = new ServiceCollection();
        configureServices?.Invoke(services);
        ServiceProvider = services.BuildServiceProvider();
        return builder;
    }

    public static IServiceProvider GetServiceProvider(this Application _)
    {
        return ServiceProvider;
    }
    public static IServiceProvider GetServiceProvider(this IClassicDesktopStyleApplicationLifetime _)
    {
        return ServiceProvider;
    }
    public static IServiceProvider GetServiceProvider(this AppDomain _)
    {
        return ServiceProvider;
    }
}


