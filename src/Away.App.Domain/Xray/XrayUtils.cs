using System.Text;

namespace Away.App.Domain.Xray;

public static class XrayUtils
{
    public static string Base64Decode(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return string.Empty;
        }

        try
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
        catch
        {
            return content;
        }
    }

    public static string UrlDecode(string content)
    {
        return HttpUtility.UrlDecode(content);
    }
}
