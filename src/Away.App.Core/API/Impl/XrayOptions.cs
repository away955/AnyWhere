namespace Away.App.Core.API.Impl;

[DI(ServiceLifetime.Singleton)]
public class XrayOptions : IXrayOptions
{
    public string IP { get; private set; } = "0.0.0.0";
    public int Port { get; private set; } = 1080;

    public void SetHost(string ip, int port)
    {
        IP = ip;
        Port = port;
    }
}
