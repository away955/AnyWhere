namespace Away.Service.DB.Entities;

/// <summary>
/// 设置表
/// </summary>
[SugarTable("sys_settings")]
public sealed class SettingsEntity
{
    /// <summary>
    /// 编号
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 键
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// 值
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Remark { get; set; } = string.Empty;
}
