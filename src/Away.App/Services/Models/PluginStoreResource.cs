namespace Away.App.Services.Models;

/// <summary>
/// 插件资源
/// </summary>
public sealed class PluginStoreResource
{
    /// <summary>
    /// 授权码
    /// </summary>
    public string Authorization { get; set; } = null!;
    /// <summary>
    /// 插件资源集合
    /// </summary>
    public List<PluginStoreEntity> Plugins { get; set; } = [];
}
