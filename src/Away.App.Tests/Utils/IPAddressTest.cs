using Away.App.Core.Utils;

namespace Away.App.Tests;


public sealed class IPAddressTest
{

    [Fact]
    public void TestTargetAddress()
    {
        var ip = IPAddressUtils.GetTarget();
        Assert.NotNull(ip);
    }
}
