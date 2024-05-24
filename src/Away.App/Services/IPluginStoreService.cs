namespace Away.App.Services;

/// <summary>
/// 插件商店服务
/// </summary>
public interface IPluginStoreService
{
    /// <summary>
    /// 获取插件列表
    /// </summary>
    /// <returns></returns>
    List<PluginStoreModel> GetList();
    /// <summary>
    /// 安装插件
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Install(PluginStoreModel model);
    /// <summary>
    /// 升级插件
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Upgrade(PluginStoreModel model);
    /// <summary>
    /// 卸载插件
    /// </summary>
    /// <param name="module">插件模块名称</param>
    bool UnInstall(string module);
    /// <summary>
    /// 禁用|启用插件
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    bool DisabledOrEnable(PluginStoreModel model);
    /// <summary>
    /// 更新插件资源
    /// </summary>
    /// <returns></returns>
    Task<bool> UpdateResource();
    /// <summary>
    /// 获取图标
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    Task<Bitmap?> GetImgae(string url);
}
