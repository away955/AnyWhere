using System.Text;

namespace Away.App.Core.Utils;

/// <summary>
/// 加密工具类
/// </summary>
public static class EncryptUtils
{
    #region Base64 加密/解密

    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Base64Encode(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }
        var bytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
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
    #endregion

}
