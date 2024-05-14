using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Away.App.Core.Utils;

public sealed class YamlUtils
{
    private static ISerializer Serializer { get; }
    private static IDeserializer Deserializer { get; }

    static YamlUtils()
    {
        Serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        Deserializer = new DeserializerBuilder()
            //.WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();


    }

    public static string Serialize<T>(T model) where T : class
    {
        return Serializer.Serialize(model);
    }

    public static T Deserialize<T>(string yaml)
    {
        return Deserializer.Deserialize<T>(yaml);
    }
}
