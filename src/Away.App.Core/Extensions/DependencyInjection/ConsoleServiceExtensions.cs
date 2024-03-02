using Away.App.Core.Windows.Consoles;
using Away.App.Core.Windows.Consoles.Impl;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class ConsoleServiceExtensions
{
    public static IServiceCollection AddConsoleManager(this IServiceCollection services, bool isShow = false)
    {
        if (OperatingSystem.IsWindows())
        {
            services.AddSingleton<IConsoleManager>(new WindowsConsoleManager(isShow));
            return services;
        }
        if (OperatingSystem.IsMacOS())
        {
            services.AddSingleton<IConsoleManager>(new MacOSConsoleManager(isShow));
            return services;
        }

        if (OperatingSystem.IsLinux())
        {
            services.AddSingleton<IConsoleManager>(new LinuxConsoleManager(isShow));
            return services;
        }
        return services;
    }
}
