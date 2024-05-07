namespace Xray;

public sealed class XraySettings : IXraySetting
{
    public string IP { get; private set; } = "0.0.0.0";
    public int Port { get; private set; } = 1080;

    public void SetHost(string ip, int port)
    {
        IP = ip;
        Port = port;
    }
}
