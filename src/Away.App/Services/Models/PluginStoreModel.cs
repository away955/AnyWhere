namespace Away.App.Services.Models;

/// <summary>
/// 插件商店模型
/// </summary>
public class PluginStoreModel
{
    /// <summary>
    /// 插件模块名称
    /// </summary>
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
    /// 当前版本
    /// </summary>
    public string CurrentVersion { get; set; } = string.Empty;
    /// <summary>
    /// 最新版本
    /// </summary>
    public string LatestVersion { get; set; } = string.Empty;
    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsDisabled { get; set; }
    /// <summary>
    /// 文件编号
    /// </summary>
    public string ContentID { get; set; } = string.Empty;
    /// <summary>
    /// 图标
    /// </summary>
    public string Logo { get; set; } = string.Empty;
    /// <summary>
    /// 文件大小/kb
    /// </summary>
    public double FileSize { get; set; }
    /// <summary>
    /// 菜单地址
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// 图片|url
    /// </summary>
    public string Image { get; set; } = string.Empty;
}