using System.Net;

namespace Away.App.Core.Utils;

public static class IPAddressUtils
{

    public static IPAddress GetTarget()
    {
        var dns = Dns.GetHostEntry(Dns.GetHostName());
        var ips = dns.AddressList.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        return ips.FirstOrDefault()!;
    }

    public static int ToInt(IPAddress ip)
    {
        return ToInt(ip.GetAddressBytes());
    }

    public static int ToInt(byte[] ipbytes)
    {
        return BitConverter.ToInt32(ipbytes.Reverse().ToArray());
    }

    public static int ToInt(string ip)
    {
        return ToInt(IPAddress.Parse(ip));
    }

    public static IPAddress ToIPAddress(int num)
    {
        return new IPAddress(BitConverter.GetBytes(num).Reverse().ToArray());
    }

    public static string ToString(int num)
    {
        return ToIPAddress(num).ToString();
    }
}
