using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Away.Service;

public partial class ProxySetting
{
    public static bool SetProxy(string proxyhost, bool proxyEnabled = true)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        try
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
            const string keyName = userRoot + @"\" + subkey;

            Registry.SetValue(keyName, "ProxyServer", proxyhost);
            Registry.SetValue(keyName, "ProxyOverride", "<local>");
            Registry.SetValue(keyName, "ProxyEnable", proxyEnabled ? "1" : "0", RegistryValueKind.DWord);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool Enabled(bool proxyEnabled = true)
    {
        return SetProxy(string.Empty, proxyEnabled);
    }
}
