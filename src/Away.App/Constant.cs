namespace Away.App;

public sealed class Constant
{
    /// <summary>
    /// App版本
    /// </summary>
    public const string Version = "v1.2.5";
    /// <summary>
    /// App名称
    /// </summary>
    public const string Title = "哪都通";
    /// <summary>
    /// App根地址
    /// </summary>
    public static string RootPath => AppDomain.CurrentDomain.BaseDirectory;
    /// <summary>
    /// App数据库服务key
    /// </summary>
    public const string DBKey = "App";
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
    /// 更新应用|插件根地址
    /// </summary>
    public const string Host = "https://proxy.v2gh.com/https://raw.githubusercontent.com/away955/anywhere/master/dist/latest";
    /// <summary>
    /// App更新
    /// </summary>
    public const string AppUpgradeResource = $"{Host}/info.md";
    /// <summary>
    /// 插件商店配置地址
    /// </summary>
    public const string PluginsStoreResource = $"{Host}/pluins.yml";
}
