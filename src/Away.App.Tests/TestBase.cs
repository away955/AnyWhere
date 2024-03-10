using Away.App.Core.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Away.App.Tests;

public abstract class TestBase
{
    protected IServiceProvider ServiceProvider { get; private set; }

    public TestBase()
    {
        var services = AwayLocator.Services;
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    public T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    protected virtual void ConfigureServices(IServiceCollection Services)
    {
        Program.ConfigureServices(Services);
        Update.Program.ConfigureServices(Services);
    }
}
