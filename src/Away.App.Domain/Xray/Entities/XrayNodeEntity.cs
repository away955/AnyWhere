namespace Away.App.Domain.Xray.Entities;

/// <summary>
/// Xray 网络节点表
/// </summary>
[SugarTable("xray_node")]
public class XrayNodeEntity
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 别名
    /// </summary>
    public string Alias { get; set; } = string.Empty;

    /// <summary>
    /// 地址
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 原Url
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    public XrayNodeStatus Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 下载速度 b/s
    /// </summary>
    public double Speed { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime Updated { get; set; } = DateTime.Now;

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
