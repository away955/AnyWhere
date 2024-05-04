using Away.App.Domain.RouterScanner;
using System.Net;

namespace Away.App.Tests;

public sealed class FingerPrintTest : TestBase
{
    [Fact]
    public void LoadProbeFileTest()
    {
        var probe = GetService<IRouterFingerPrintHub>();
        Assert.NotNull(probe.Matches);
    }


    [InlineData("192.168.2.1:80")]
    [Theory]
    public async Task RedmiTest(string host)
    {
        await FingerPrintScannerTest(host);
    }

    [InlineData("192.141.183.244:8085")] // WR740N/WR741ND
    [InlineData("200.218.252.81:1080")] // MR3420
    [InlineData("74.126.121.76:4433")] // WR940N
    [InlineData("183.106.38.182:3000")] // WR940N plus
    [Theory]
    public async Task TPLinkTest(string host)
    {
        await FingerPrintScannerTest(host);
    }

    [InlineData("192.168.1.1:80")]
    [Theory]
    public async Task ChinaMobileTest(string host)
    {
        await FingerPrintScannerTest(host);
    }

    private async Task FingerPrintScannerTest(string host)
    {
        var server = GetService<IRouterFingerPrintScanner>();
        server.Timeout = 1000 * 5;
        server.Host = IPEndPoint.Parse(host);
        var res = await server.Run();
        Assert.True(res.Success);
    }


}
