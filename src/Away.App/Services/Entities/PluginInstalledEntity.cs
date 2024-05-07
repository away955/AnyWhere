namespace Away.App.Services.Entities;

/// <summary>
/// 插件注册表
/// </summary>
[SugarTable("app_pluign_installed")]
public sealed class PluginInstalledEntity
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
    /// 是否禁用
    /// </summary>
    public bool IsDisabled { get; set; }
}
