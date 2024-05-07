namespace Xray.Services;

public static class XrayUtils
{
    public static string Base64Decode(string content)
    {
        return EncryptUtils.Base64Decode(content);
    }

    public static string UrlDecode(string content)
    {
        return HttpUtility.UrlDecode(content);
    }  
}
