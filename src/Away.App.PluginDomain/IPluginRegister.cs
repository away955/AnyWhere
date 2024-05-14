namespace Away.App.PluginDomain;

/// <summary>
/// 插件注册
/// </summary>
public interface IPluginRegister : IDisposable
{
    /// <summary>
    /// 插件模块名
    /// </summary>
    string Module { get; }

    /// <summary>
    /// 服务注册
    /// </summary>
    /// <param name="services"></param>
    IServiceCollection ConfigureServices(IServiceCollection services);
    /// <summary>
    /// 应用启动
    /// </summary>
    void ApplicationStartup();
    /// <summary>
    /// 应用退出
    /// </summary>
    void ApplicationExit();
}
