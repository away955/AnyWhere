using Away.App.Update.Services;

namespace Away.App.Tests;

public sealed class VersionServiceTest : TestBase
{
    [Fact]
    public void TestGetVersionInfo()
    {
        var service = GetService<IVersionService>();
        service.GetVersionInfo();
    }
}
