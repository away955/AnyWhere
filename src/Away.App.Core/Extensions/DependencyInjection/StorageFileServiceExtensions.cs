using Away.App.Core.Extensions.App;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class StorageFileServiceExtensions
{
    public static IServiceCollection AddStorageFile(this IServiceCollection services)
    {
        return services.AddSingleton(o => Application.Current?.GetTopLevel()?.StorageProvider!);
    }
}
