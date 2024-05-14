using Away.App.Core.Utils;

namespace Away.App.Tests;

public sealed class YamlTest
{
    [Fact]
    public void TestSerialize()
    {
        var yaml = YamlUtils.Serialize(new TestYamlModel());
        Assert.NotNull(yaml);
    }

    [Fact]
    public void TestDeserialize()
    {
        var yaml = """
            # 测试
            name: away
            number: 110
            isok: true
            items:
            - a
            - b
            - c
            values:
            - 1
            - 2
            - 3
            """;
        var model = YamlUtils.Deserialize<TestYamlModel>(yaml);
        Assert.NotNull(model);
    }

    public class TestYamlModel
    {
        public string Name { get; set; } = "away";
        public int Number { get; set; } = 110;
        public bool Isok { get; set; } = true;
        public string[] Items { get; set; } = ["a", "b", "c"];
        public int[] Values { get; set; } = [1, 2, 3];
    }
}
