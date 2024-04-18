namespace Away.App.Models;

public sealed class XrayTestSettingsModel
{
    /// <summary>
    /// 检测地址
    /// </summary>
    [Reactive]
    public string TestUrl { get; set; } = null!;
    /// <summary>
    /// 检测超时时间
    /// </summary>
    [Reactive]
    public int TestTimeout { get; set; }
    /// <summary>
    /// 线程数
    /// </summary>
    [Reactive]
    public int Concurrency { get; set; }
    /// <summary>
    /// 检测起始端口
    /// </summary>
    [Reactive]
    public int StartPort { get; set; }
}
