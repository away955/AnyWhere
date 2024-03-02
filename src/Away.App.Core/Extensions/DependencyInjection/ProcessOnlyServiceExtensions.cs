using Away.App.Core.Windows.ProcessOnly;
using Away.App.Core.Windows.ProcessOnly.Impl;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class ProcessOnlyServiceExtensions
{
    public static IServiceCollection AddProcessOnly(this IServiceCollection services, bool isShow = false)
    {
        if (OperatingSystem.IsWindows())
        {
            services.AddTransient<IProcessOnly, WindowsProcessOnly>();
            return services;
        }
        if (OperatingSystem.IsMacOS())
        {
            return services;
        }

        if (OperatingSystem.IsLinux())
        {
            return services;
        }
        return services;
    }
}
