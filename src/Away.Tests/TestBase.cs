using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Away.Tests;

public abstract class TestBase
{
    protected IConfiguration Configuration { get; private set; }
    protected ServiceProvider ServiceProvider { get; private set; }

    public TestBase()
    {
        var services = new ServiceCollection();
        AddServiceCollections(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    public T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    private void AddServiceCollections(IServiceCollection Services)
    {
        Configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();


        Services.AddSingleton(Configuration);
        Services.AddHttpClient("xray")
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
            });
        var connStr = Configuration.GetConnectionString("Sqlite");
        Services.AddSqlSugarClient(connStr!);
        Services.AddAwayDI();
    }
}
