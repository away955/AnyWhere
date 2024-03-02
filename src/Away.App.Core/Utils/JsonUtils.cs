using Away.App.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Away.App.Core.Utils;

public static class JsonUtils
{
    private static JsonSerializerOptions? _jsonOptions;
    private static JsonSerializerOptions JsonOptions
    {
        get
        {
            if (_jsonOptions == null)
            {
                var provider = AppDomain.CurrentDomain.GetServiceProvider();
                _jsonOptions = provider.GetService<IOptions<JsonSerializerOptions>>()!.Value;
            }
            return _jsonOptions;
        }
    }

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
