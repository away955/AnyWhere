namespace Away.Service.DB.Entities;

/// <summary>
/// 菜单表
/// </summary>
[SugarTable("sys_menu")]
public sealed class MenuEntity
{
    /// <summary>
    /// 编号
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 图标
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 路径
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Url { get; set; } = string.Empty;
}

