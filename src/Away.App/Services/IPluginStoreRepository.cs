namespace Away.App.Services;

/// <summary>
/// 已安装的插件仓储
/// </summary>
public interface IPluginStoreRepository
{
    /// <summary>
    /// 获取插件列表
    /// </summary>
    /// <returns></returns>
    List<PluginStoreModel> GetList();
    /// <summary>
    /// 获取已注册的插件
    /// </summary>
    /// <returns></returns>
    List<PluginRegisterEntity> GetPluginRegisters();
    /// <summary>
    /// 注册插件
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool Register(PluginRegisterEntity entity);
    /// <summary>
    /// 取消注册插件
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    bool UnRegister(string module);
    /// <summary>
    /// 更新或添加插件资源
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    bool Save(List<PluginStoreEntity> list);
}
