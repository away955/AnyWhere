using Away.App.PluginDomain;
using Away.App.Services.Impl;
using System.Reflection;

namespace Away.App.Services;

/// <summary>
/// 插件注册管理
/// </summary>
public sealed class PluginRegisterManager
{
    public static void Register()
    {
        //LoadPlugin("Xray");
        //LoadPlugin("Youtube");

        RegisterService(AwayLocator.Services);
        using var provider = AwayLocator.Services.BuildServiceProvider();
        var registerRep = provider.GetRequiredService<IPluginInstalledRepository>();
        var plugins = registerRep.GetListByNotDisabled();
        foreach (var plugin in plugins)
        {
            LoadPlugin(plugin.Module);
        }
    }

    private static void RegisterService(IServiceCollection services)
    {
        services.AddTransient<IPluginInstalledRepository, PluginInstalledRepository>();
    }

    private static void LoadPlugin(string module)
    {
        Log.Information($"注册插件：{module}");
        var dir = Path.Combine(Constant.PluginsRootPath, module);
        var files = Directory.GetFiles(dir);
        foreach (var file in files.Where(o => o.EndsWith("dll")))
        {
            Load(file, module);
        }
    }

    private static void Load(string dllPath, string module)
    {
        var assembly = Assembly.LoadFrom(dllPath);
        if (dllPath.Contains($"{module}.dll"))
        {
            foreach (var type in assembly.DefinedTypes)
            {
                if (type.GetInterface(nameof(IPluginRegister)) != null)
                {
                    if (Activator.CreateInstance(type) is not IPluginRegister register)
                    {
                        break;
                    }
                    register.ConfigureServices(AwayLocator.Services);
                    AwayLocator.Services.AddKeyedSingleton(Constant.PluginRegisterServiceKey, register);
                    break;
                }
            }
        }

        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            return assembly;
        };
    }
}
