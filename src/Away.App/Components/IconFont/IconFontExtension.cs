using System.Text.RegularExpressions;

namespace Away.App.Components.IconFont;

public class EnumExtension(IconType key) : KeyExtension(key.ToString())
{
}

public class KeyExtension(string key) : MarkupExtension
{
    public string Key { get; set; } = key;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var text = IconFont.ResourceManager.GetString(Key) ?? string.Empty;
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
