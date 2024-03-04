using Away.App.Domain.XrayNodeSub;

namespace Away.App.Tests.Xray;

public class XrayNodeSubTest : TestBase
{
    [Theory]
    [InlineData("https://clashgithub.com/wp-content/uploads/rss/${date:yyyyMMdd}.txt")]
    public void ParseUrl(string url)
    {
        var entity = new XrayNodeSubEntity()
        {
            Url = url
        };
        var item = entity.ParseUrl();
        Assert.NotEqual(item, url);
    }
}
