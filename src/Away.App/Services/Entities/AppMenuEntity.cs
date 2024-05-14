namespace Away.App.Services.Entities;

/// <summary>
/// App菜单表
/// </summary>
[SugarTable("app_menu")]
public sealed class AppMenuEntity
{
    /// <summary>
    /// 菜单地址
    /// </summary>
    [SugarColumn(IsPrimaryKey = true)]
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// 所属插件模块
    /// </summary>
    public string Module { get; set; } = string.Empty;
    /// <summary>
    /// 菜单标题
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// 菜单图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;
    /// <summary>
    /// 排序
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsDisabled { get; set; }
}
