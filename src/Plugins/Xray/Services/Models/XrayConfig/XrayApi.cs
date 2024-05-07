namespace Xray.Services.Models;

/// <summary>
/// 提供了一些 API 接口供远程调用
/// </summary>
public sealed class XrayApi
{
    /// <summary>
    /// 出站代理标识
    /// </summary>
    public string tag { get; set; } = string.Empty;
    /// <summary>
    /// 开启的 API 列表
    /// HandlerService
    /// 一些对于入站出站代理进行修改的 API，可用的功能如下：
    /// 添加一个新的入站代理；
    /// 添加一个新的出站代理；
    /// 删除一个现有的入站代理；
    /// 删除一个现有的出站代理；
    /// 在一个入站代理中添加一个用户（仅支持 VMess、VLESS、Trojan、Shadowsocks（v1.3.0+））；
    /// 在一个入站代理中删除一个用户（仅支持 VMess、VLESS、Trojan、Shadowsocks（v1.3.0+））；
    /// 
    /// LoggerService
    /// 支持对内置 Logger 的重启，可配合 logrotate 进行一些对日志文件的操作。
    /// 
    /// StatsService
    /// 内置的数据统计服务，详见 统计信息。
    /// 
    /// ReflectionService
    /// 支持 gRPC 客户端获取服务端的 API 列表。
    /// </summary>
    public List<string> services { get; set; } = [];

    public static XrayApi Default
    {
        get => new()
        {
            tag = "api",
            services = ["HandlerService", "LoggerService", "ReflectionService"]
        };
    }
}
