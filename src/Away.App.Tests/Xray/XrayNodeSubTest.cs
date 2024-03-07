using Away.App.Domain.XrayNodeSub;
using Away.Domain.XrayNode;

namespace Away.App.Tests.Xray;

public class XrayNodeSubTest : TestBase
{
    [Theory]
    [InlineData("https://clashgithub.com/wp-content/uploads/rss/${date:yyyyMMdd}.txt")]
    public void TestParseUrl(string url)
    {
        var entity = new XrayNodeSubEntity()
        {
            Url = url
        };
        var item = entity.ParseUrl();
        Assert.NotEqual(item, url);
    }

    [Theory]
    [InlineData("https://clashgithub.com/wp-content/uploads/rss/${date:yyyyMMdd}.txt")]
    [InlineData("https://proxy.v2gh.com/https://raw.githubusercontent.com/mksshare/mksshare.github.io/main/README.md")]
    public async void TestGetNodes(string url)
    {
        var subSerivce = GetService<IXrayNodeSubService>();
        var xrayservice = GetService<IXrayNodeService>();

        var nodes = await subSerivce.GetXrayNode(url);
        xrayservice.SaveNodes(nodes);
        Assert.NotEmpty(nodes);
    }
}
