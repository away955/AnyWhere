namespace Xray.Services.Impl;

public abstract class ProxySettingBase
{
    private const string ALL_PROXY = "ALL_PROXY";
    private const string NO_PROXY = "NO_PROXY";

    protected (string server, string noProxy) Get()
    {
        var server = Environment.GetEnvironmentVariable(ALL_PROXY) ?? string.Empty;
        var noProxy = Environment.GetEnvironmentVariable(NO_PROXY) ?? string.Empty;
        return (server, noProxy);
    }

    protected bool Set(string server, string noProxy, bool isEnable)
    {
        if (isEnable)
        {
            Environment.SetEnvironmentVariable(ALL_PROXY, server);
            Environment.SetEnvironmentVariable(NO_PROXY, noProxy);
        }
        else
        {
            Environment.SetEnvironmentVariable(ALL_PROXY, null);
            Environment.SetEnvironmentVariable(NO_PROXY, null);
        }
        return true;
    }
}
