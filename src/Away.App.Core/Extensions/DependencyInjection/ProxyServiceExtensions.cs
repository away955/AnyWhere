using Away.App.Core.Windows.Proxy;
using Away.App.Core.Windows.Proxy.Impl;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class ProxyServiceExtensions
{
    public static IServiceCollection AddProxySettings(this IServiceCollection services)
    {
        if (OperatingSystem.IsWindows())
        {
            services.AddScoped<IProxySetting, WindowsProxySetting>();
            return services;
        }
        if (OperatingSystem.IsMacOS())
        {
            services.AddScoped<IProxySetting, MacOSProxySetting>();
            return services;
        }

        if (OperatingSystem.IsLinux())
        {
            services.AddScoped<IProxySetting, LinuxProxySetting>();
            return services;
        }
        return services;
    }
}
