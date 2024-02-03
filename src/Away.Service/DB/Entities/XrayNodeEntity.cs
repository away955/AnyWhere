namespace Away.Service.DB.Entities;

/// <summary>
/// Xray 网络节点表
/// </summary>
[SugarTable("xray_node")]
public class XrayNodeEntity
{
    /// <summary>
    /// 编号
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 别名
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Alias { get; set; } = string.Empty;

    /// <summary>
    /// 地址
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 原Url
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    public XrayNodeStatus Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(DefaultValue = "")]
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 是否使用
    /// </summary>
    public bool IsChecked { get; set; }
}

public enum XrayNodeStatus
{
    /// <summary>
    /// 不可用
    /// </summary>
    Error = -1,
    /// <summary>
    /// 未检测
    /// </summary>
    Default = 0,
    /// <summary>
    /// 可用
    /// </summary>
    Success = 1,
}
