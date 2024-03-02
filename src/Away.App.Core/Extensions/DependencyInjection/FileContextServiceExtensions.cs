using Away.App.Core.Repository;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class FileContextServiceExtensions
{
    public static IServiceCollection AddFileContext(this IServiceCollection services, Action<FileContextOptions>? action = null)
    {
        FileContextOptions options = new();
        action?.Invoke(options);
        return services.AddSingleton<IFileContext>(o =>
        {
            return new FileContext(options);
        });
    }
}
