using Away.App.Core.DI;
using Away.App.Domain.RouterScanner;

namespace Away.App.Tests;

public sealed class VulHubTest : TestBase
{
    private static IEnumerable<IRouterVulHub> VULHUBS => AwayLocator.GetKeyedServices<IRouterVulHub>(Constant.VulHubKey);

    [InlineData("CVE-2019-6971", "http://82.79.101.200:8081")]
    [InlineData("CVE-2023-36355", "http://197.162.239.174:80")]
    [InlineData("CVE-2023-1389", "http://185.41.80.115:1080")]
    [InlineData("CVE-2018-12692", "http://81.12.144.46")]
    [InlineData("CVE-2020-24363", "http://185.41.80.115:1080")]
    [InlineData("CVE-2020-9374", "http://185.41.80.115:1080")]
    [Theory]
    public async Task CVETest(string cve, string url)
    {
        var vulhub = VULHUBS.FirstOrDefault(o => o.CVE == cve);
        if (vulhub == null)
        {
            Assert.NotNull(vulhub);
        }
        vulhub.Url = url;
        var res = await vulhub.Poc();
        Assert.True(res.Success);
    }
}
