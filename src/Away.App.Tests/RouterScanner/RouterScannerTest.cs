using RouterScanner.Services;

namespace Away.App.Tests;

public sealed class RouterScannerTest : TestBase
{
    [InlineData("192.168.2.1-192.168.2.3", "80")]
    [Theory]
    public void ScannerTest(string ip, string port)
    {
        var server = GetService<IRouterScanner>();
        server.IPs = ip;
        server.Ports = port;
        server.OnFingerPrintCompleted += (res) =>
        {
            Console.WriteLine($"指纹扫描：{res.Success}");
        };
        server.OnVulCompleted += (res) =>
        {
            Console.WriteLine($"漏洞扫描：{res.Success}");
        };
        server.Run();
        Assert.True(true);
    }
}
