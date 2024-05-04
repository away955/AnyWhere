using Away.App.Domain.Xray;

namespace Away.App.Tests;

public class XrayNodeSubTest : TestBase
{
    [Theory]
    [InlineData("https://proxy.v2gh.com/https://raw.githubusercontent.com/mksshare/mksshare.github.io/main/README.md")]
    public async Task TestGetNodes(string url)
    {
        var subSerivce = GetService<IXrayNodeSubService>();
        var nodes = await subSerivce.GetXrayNode(url);
        Assert.NotEmpty(nodes);
    }
}
