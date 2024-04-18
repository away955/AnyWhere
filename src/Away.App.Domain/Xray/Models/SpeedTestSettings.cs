namespace Away.App.Domain.Xray.Models;

public sealed class SpeedTestSettings
{
    /// <summary>
    /// 检测地址
    /// </summary>
    public string TestUrl { get; set; } = "http://www.x.com";
    /// <summary>
    /// 检测超时时间
    /// </summary>
    public int TestTimeout { get; set; } = 25;
    /// <summary>
    /// 线程数
    /// </summary>
    public int Concurrency { get; set; } = 5;
    /// <summary>
    /// 检测起始端口
    /// </summary>
    public int StartPort { get; set; } = 3000;
}
