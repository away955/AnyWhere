namespace Away.App.PluginDomain;

public abstract class PluginRegisterBase<TPlugin> where TPlugin : IPluginRegister
{
    public PluginRegisterBase()
    {
    }

    /// <summary>
    /// 服务注册
    /// </summary>
    /// <param name="services"></param>
    public abstract void ConfigureServices(IServiceCollection services);

    /// <summary>
    /// 应用启动
    /// </summary>
    public virtual void ApplicationStartup()
    {
    }


    /// <summary>
    /// 应用退出
    /// </summary>
    public virtual void ApplicationExit()
    {
    }
}
