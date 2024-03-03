using Microsoft.Extensions.Options;

namespace Away.App.Core.Utils;

public static class JsonUtils
{
    private static JsonSerializerOptions? _jsonOptions;
    private static JsonSerializerOptions JsonOptions => _jsonOptions ??= AwayLocator.ServiceProvider.GetService<IOptions<JsonSerializerOptions>>()!.Value;

    public static string Serialize<T>(T t)
    {
        return JsonSerializer.Serialize(t, JsonOptions);
    }

    public static byte[] SerializeToUtf8Bytes<TValue>(TValue value)
    {
        return JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);
    }
    public static T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }
    public static T? Deserialize<T>(byte[]? json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }
}
