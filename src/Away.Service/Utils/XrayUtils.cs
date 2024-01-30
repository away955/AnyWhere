using System.Text;
using System.Text.Json.Serialization;
using System.Web;

namespace Away.Service.Utils;

public static class XrayUtils
{
    private static JsonSerializerOptions _jsonSerializerOptions;
    static XrayUtils()
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        };
    }

    public static string Serialize<T>(T t)
    {
        return JsonSerializer.Serialize(t, _jsonSerializerOptions);
    }
    public static T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
    }

    public static string Base64Decode(string content)
    {
        switch (content.Length % 4)
        {
            case 2:
                content += "==";
                break;
            case 3:
                content += "=";
                break;

        }
        byte[] bytes = Convert.FromBase64String(content);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string UrlDecode(string content)
    {
        return HttpUtility.UrlDecode(content);
    }
}
