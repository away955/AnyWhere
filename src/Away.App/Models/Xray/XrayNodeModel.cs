using Away.App.Domain.Xray.Entities;

namespace Away.App.Models;

public sealed class XrayNodeModel : ViewModelBase
{
    [Reactive]
    public int Id { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    [Reactive]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 别名
    /// </summary>
    [Reactive]
    public string Alias { get; set; } = string.Empty;

    /// <summary>
    /// 地址
    /// </summary>
    [Reactive]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// 端口
    /// </summary>
    [Reactive]
    public int Port { get; set; }

    /// <summary>
    /// 原Url
    /// </summary>
    [Reactive]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    [Reactive]
    public XrayNodeStatus Status { get; set; }

    /// <summary>
    /// 下载速度 b/s
    /// </summary>
    [Reactive]
    public double Speed { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Reactive]
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 是否使用
    /// </summary>
    [Reactive]
    public bool IsChecked { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Reactive]
    public DateTime Updated { get; set; }
}
