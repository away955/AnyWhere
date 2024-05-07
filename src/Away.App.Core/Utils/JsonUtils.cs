using System.Text.Json.Serialization;

namespace Away.App.Core.Utils;

public static class JsonUtils
{
    private static JsonSerializerOptions? _jsonOptions;
    private static JsonSerializerOptions JsonOptions => _jsonOptions ??= new JsonSerializerOptions
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

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
