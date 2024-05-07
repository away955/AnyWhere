namespace Xray.Services.Models;

/// <summary>
/// 反向代理。可以把服务器端的流量向客户端转发，即逆向流量转发
/// </summary>
public sealed class XrayReverse
{
    public List<ReverseItem>? bridges { get; set; }
    public List<ReverseItem>? portals { get; set; }   
}

public sealed class ReverseItem
{
    public string? tag { get; set; }
    public string? domain { get; set; }
}
