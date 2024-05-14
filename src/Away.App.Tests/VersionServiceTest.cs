using Away.App.Services;

namespace Away.App.Tests;

public sealed class VersionServiceTest : TestBase
{
    [Theory]
    [InlineData("https://gitee.com/wyj95_admin/anywhere/raw/master/dist/latest/info.md")]
    [InlineData("https://proxy.v2gh.com/https://raw.githubusercontent.com/away955/anywhere/master/dist/latest/info.md")]
    public async void TestGetVersionInfo(string url)
    {
        var service = GetService<IVersionService>();
        var res = await service.GetVersionInfo(url);
        Assert.True(!string.IsNullOrWhiteSpace(res.Version));
    }
}
