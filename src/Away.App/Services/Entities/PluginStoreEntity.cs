namespace Away.App.Services.Entities;

/// <summary>
/// 插件商店表
/// </summary>
[SugarTable("app_pluign_store")]
public sealed class PluginStoreEntity
{
    /// <summary>
    /// 插件模块名称
    /// </summary>
    [SugarColumn(IsPrimaryKey = true)]
    public string Module { get; set; } = string.Empty;
    /// <summary>
    /// 插件名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; } = string.Empty;
    /// <summary>
    /// 图片|url
    /// </summary>
    public string Image { get; set; } = string.Empty;
    /// <summary>
    /// 文件大小/kb
    /// </summary>
    public double FileSize { get; set; }
    /// <summary>
    /// 文件编号
    /// </summary>
    public string ContentID { get; set; } = string.Empty;
    /// <summary>
    /// 菜单地址
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// 图标|unicode
    /// </summary>
    public string Logo { get; set; } = string.Empty;
}
