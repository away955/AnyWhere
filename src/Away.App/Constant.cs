namespace Away.App;

public sealed class Constant
{
    static Constant()
    {
        var filename = OperatingSystem.IsWindows() ? "Away.App.exe" : "Away.App";
        var app = System.Diagnostics.FileVersionInfo.GetVersionInfo(Path.Combine(RootPath, filename));
        Version = $"v{app.FileVersion}";
    }
    /// <summary>
    /// App版本
    /// </summary>
    public static string Version { get; private set; }
    /// <summary>
    /// App名称
    /// </summary>
    public const string Title = "哪都通";

    /// <summary>
    /// App根地址
    /// </summary>
    public static string RootPath => AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// App数据库连接字符串
    /// </summary>
    public static string DBConn => $"DataSource={Path.Combine(RootPath, "app.sqlite")}";
    /// <summary>
    /// 插件服务key
    /// </summary>
    public const string PluginRegisterServiceKey = "PluginRegister";
    /// <summary>
    /// 插件根地址
    /// </summary>
    public static string PluginsRootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
    /// <summary>
    /// 插件商店配置地址
    /// </summary>
    public const string PluginsStoreResource = "https://proxy.v2gh.com/https://raw.githubusercontent.com/away955/anywhere/master/dist/latest/pluins.yml";
}
