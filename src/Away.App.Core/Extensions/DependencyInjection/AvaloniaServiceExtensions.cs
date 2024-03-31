using Away.App.Core.Extensions.App;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class AvaloniaServiceExtensions
{
    public static IServiceCollection AddClipboard(this IServiceCollection services)
    {
        return services.AddSingleton(o => Application.Current?.GetTopLevel()?.Clipboard!);
    }
    public static IServiceCollection AddStorageProvider(this IServiceCollection services)
    {
        return services.AddSingleton(o => Application.Current?.GetTopLevel()?.StorageProvider!);
    }
}
