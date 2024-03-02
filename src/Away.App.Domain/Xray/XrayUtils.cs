using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Away.App.Domain.Xray;


public static class XrayUtils
{
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
