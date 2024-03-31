using Avalonia.Markup.Xaml;
using System.Text.RegularExpressions;

namespace Away.App.Components.IconFont;

public sealed class EnumExtension(IconType key) : KeyExtension(key.ToString())
{
}

public class KeyExtension(string key) : MarkupExtension
{
    public string Key { get; set; } = key;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (!IconData.Current.TryGetValue(Key, out var text))
        {
            return string.Empty;
        }
        return text.ToUnicode();
    }
}


public static class IconFontExtension
{
    public static string ToUnicode(this string text)
    {
        return Regex.Unescape(text.Replace("&#xe", "\\ue").Replace(";", ""));
    }
}
