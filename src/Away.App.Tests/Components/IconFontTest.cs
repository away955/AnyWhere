using Away.App.Components.IconFont;

namespace Away.App.Tests.Components;

public sealed class IconFontTest : TestBase
{

    [Fact]
    public void TestIconType()
    {
        _ = IconData.Current.TryGetValue("Home", out string? val);
        Assert.Equal("&#xe666;", val);
    }
}
