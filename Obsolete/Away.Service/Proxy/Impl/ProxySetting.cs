using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Away.Service.Proxy.Impl;

[ServiceInject]
public class ProxySetting : IProxySetting
{
    const string userRoot = "HKEY_CURRENT_USER";
    const string subkey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
    const string keyName = userRoot + @"\" + subkey;

    public string ProxyServer { get; set; } = string.Empty;
    public string ProxyOverride { get; set; } = "<local>";
    public bool ProxyEnable { get; set; }


    public ProxySetting()
    {
        GetProxy();
    }

    public bool Save()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        try
        {
            Registry.SetValue(keyName, nameof(ProxyServer), ProxyServer);
            Registry.SetValue(keyName, nameof(ProxyOverride), ProxyOverride);
            Registry.SetValue(keyName, nameof(ProxyEnable), ProxyEnable ? "1" : "0", RegistryValueKind.DWord);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void GetProxy()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        ProxyServer = Convert.ToString(Registry.GetValue(keyName, nameof(ProxyServer), string.Empty))!;
        ProxyOverride = Convert.ToString(Registry.GetValue(keyName, nameof(ProxyOverride), string.Empty))!;
        ProxyEnable = Convert.ToBoolean(Registry.GetValue(keyName, nameof(ProxyEnable), false));
    }
}