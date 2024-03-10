namespace Away.App.Domain.XrayNodeSub;

/// <summary>
/// 节点订阅表
/// </summary>
public class XrayNodeSubEntity
{
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
