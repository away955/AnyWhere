namespace Away.App.Services;

/// <summary>
/// App菜单仓储
/// </summary>
public interface IAppMenuRepository
{
    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    List<AppMenuEntity> GetList();
    /// <summary>
    /// 保存菜单
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool Save(AppMenuEntity entity);
    /// <summary>
    /// 删除插件菜单
    /// </summary>
    /// <param name="module">插件模块名称</param>
    /// <returns></returns>
    bool DeleteByModule(string module);
    /// <summary>
    /// 禁用或启动
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    bool IsDisabledByModule(string module);
}
