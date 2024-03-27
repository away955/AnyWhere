using SqlSugar;

namespace Away.App.Domain.XrayNode.Entities;

/// <summary>
/// 节点订阅表
/// </summary>
[SugarTable("xray_node_sub")]
public class XrayNodeSubEntity
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 订阅地址
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsDisable { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;
}
