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
    /// 加密方式
    /// </summary>
    public string security { get; set; } = string.Empty;

    /// <summary>
    /// 传输协议
    /// </summary>
    public string Transport { get; set; } = string.Empty;

    /// <summary>
    /// 安全协议
    /// </summary>
    public string TLS { get; set; } = string.Empty;

    /// <summary>
    /// 原Url
    /// </summary>
    public string Url { get; set; } = string.Empty;
}
