namespace Away.Service.Xray.Model;

/// <summary>
/// 日志配置，控制 Xray 输出日志的方式.
/// </summary>
public class XrayLog
{
    /// <summary>
    /// 访问日志的文件地址，其值是一个合法的文件地址，
    /// 如"/var/log/Xray/access.log"（Linux）
    /// 或者"C:\\Temp\\Xray\\_access.log"（Windows）。
    /// 当此项不指定或为空值时，表示将日志输出至 stdout。
    /// 特殊值none，即关闭 access log
    /// </summary>
    public string access { get; set; } = string.Empty;

    /// <summary>
    /// 错误日志的文件地址，其值是一个合法的文件地址，
    /// 如"/var/log/Xray/error.log"（Linux）
    /// 或者"C:\\Temp\\Xray\\_error.log"（Windows）。
    /// 当此项不指定或为空值时，表示将日志输出至 stdout。
    /// 特殊值none，即关闭 error log。
    /// </summary>
    public string error { get; set; } = string.Empty;

    /// <summary>
    /// error: 日志的级别, 指示 error 日志需要记录的信息. 默认值为 "warning"。
    /// debug：调试程序时用到的输出信息。同时包含所有 "info" 内容。
    /// info：运行时的状态信息等，不影响正常使用。同时包含所有 "warning" 内容。
    /// warning：发生了一些并不影响正常运行的问题时输出的信息，但有可能影响用户的体验。同时包含所有 "error" 内容。
    /// error：Xray 遇到了无法正常运行的问题，需要立即解决。
    /// none：不记录任何内容。
    /// </summary>
    public string loglevel { get; set; } = "warning";

    /// <summary>
    /// 是否启用 DNS 查询日志
    /// </summary>
    public bool dnsLog { get; set; }


    public static XrayLog Default { get => new(); }
}
