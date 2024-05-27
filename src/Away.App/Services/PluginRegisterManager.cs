using System.Reflection;
using System.Runtime.Loader;

namespace Away.App.Services;

/// <summary>
/// 插件程序集
/// </summary>
public sealed class PluginAssembly
{
    /// <summary>
    /// 程序集
    /// </summary>
    public required Assembly Assembly { get; set; }
    /// <summary>
    /// 程序集名称
    /// </summary>
    public required string AssemblyName { get; set; }
    /// <summary>
    /// 插件模块名称
    /// </summary>
    public required string Module { get; set; }
}

/// <summary>
/// 插件注册管理
/// </summary>
public sealed class PluginRegisterManager
{
    private static List<PluginAssembly> Assemblies { get; set; } = [];

    static PluginRegisterManager()
    {
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            var module = args!.Name.Split(",")[0];
            return AssemblyLoadContext.Default.Assemblies.FirstOrDefault(o => o.GetName().Name == module)
             ?? Assemblies.FirstOrDefault(o => o.AssemblyName.StartsWith(module))?.Assembly;
        };

        AwayLocator.Services.AddTransient<IPluginStoreRepository, PluginStoreRepository>();
    }


    /// <summary>
    /// 卸载插件
    /// </summary>
    /// <param name="module">插件模块名称</param>
    public static void UnLoadPlugin(string module)
    {
        Assemblies.RemoveAll(o => o.Module == module);
        Log.Information($"卸载插件：{module}");
    }

    /// <summary>
    /// 注册所有启动的插件
    /// </summary>
    public static void Register()
    {
        using var provider = AwayLocator.Services.BuildServiceProvider();
        var registerRep = provider.GetRequiredService<IPluginStoreRepository>();
        var plugins = registerRep.GetPluginRegisters();

        foreach (var plugin in plugins)
        {
            try
            {
                LoadPlugin(plugin.Module);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"加载插件失败：{plugin.Module}");
            }
        }
    }


    /// <summary>
    /// 加载插件
    /// </summary>
    /// <param name="module"></param>
    public static void LoadPlugin(string module)
    {
        Log.Information($"加载插件：{module} ...");

        var floder = Path.Combine(Constant.PluginsRootPath, module);

        var dlls = Directory.GetFiles(floder).AsEnumerable().Where(o => o.EndsWith("dll")).Reverse();
        foreach (var assemblyPath in dlls)
        {
            var name = Path.GetFileNameWithoutExtension(assemblyPath);
            var bytes = File.ReadAllBytes(assemblyPath);
            var assembly = AppDomain.CurrentDomain.Load(bytes);

            Assemblies.Add(new PluginAssembly
            {
                Assembly = assembly,
                Module = module,
                AssemblyName = name,
            });

            if (assemblyPath.EndsWith($"{module}.dll"))
            {
                CreatePluginRegister(assembly);
            }
        }
    }

    private static IPluginRegister? CreatePluginRegister(Assembly assembly)
    {
        IPluginRegister? register = null;
        foreach (var type in assembly.GetTypes())
        {
            if (type.GetInterface(nameof(IPluginRegister)) != null)
            {
                var obj = Activator.CreateInstance(type);
                if (obj is IPluginRegister service)
                {
                    register = service;
                    break;
                }
            }
        }
        if (register == null)
        {
            return register;
        }

        register.ConfigureServices(AwayLocator.Services);
        AwayLocator.Services.AddKeyedSingleton<IPluginRegister>(Constant.PluginRegisterServiceKey, register);
        Log.Information($"加载插件：{register.Module} 完成！");
        return null;
    }


    //private sealed class PluginAssemblyLoadContext : AssemblyLoadContext
    //{
    //    public PluginAssemblyLoadContext(string name) : base(name, true)
    //    {

    //    }

    //    protected override Assembly? Load(AssemblyName assemblyName)
    //    {
    //        var assembly = Default.Assemblies.FirstOrDefault(o => o.GetName().Name == assemblyName.Name);
    //        if (assembly != null)
    //        {
    //            return assembly;
    //        }
    //        Log.Information($"load:{assemblyName.FullName}");
    //        return PluginRegisterManager.Assemblies.FirstOrDefault(o => o.AssemblyName == assemblyName.Name)?.Assembly;
    //    }

    //    protected override nint LoadUnmanagedDll(string unmanagedDllName)
    //    {
    //        Log.Information($"uload:{unmanagedDllName}");
    //        return 0;
    //    }

    //}
}