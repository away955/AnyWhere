namespace Away.App.Core.Extensions;

public static class StringExtensions
{
    public static string ToUnicode(this string text)
    {
        return Regex.Unescape(text.Replace("&#xe", "\\ue").Replace(";", ""));
    }
}
