namespace Xray;

/// <summary>
/// Xray 公共接口
/// </summary>
public interface IXraySetting
{
    string IP { get; }
    int Port { get; }
    string Host => $"{IP}:{Port}";
    void SetHost(string ip, int port);
}
