namespace Away.App.Core.Extensions.DependencyInjection;

public static class AppBuilderServiceExtensions
{
    public static AppBuilder UseAwayLocator(this AppBuilder builder, Action<IServiceCollection> configureServices)
    {       
        configureServices?.Invoke(AwayLocator.Services);
        return builder;
    }
}


