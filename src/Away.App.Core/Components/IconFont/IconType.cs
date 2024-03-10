using System.Collections.ObjectModel;
using System.Reflection;

namespace Away.App.Components.IconFont;

public enum IconType
{
    [IconText("&#xe666;")] Home,
    [IconText("&#xe65a;")] Logo,
    [IconText("&#xe646;")] Close,
    [IconText("&#xe606;")] Maximum,
    [IconText("&#xe621;")] Minimal,
    [IconText("&#xe6b7;")] Normal,
    [IconText("&#xe619;")] Fly,
    [IconText("&#xe686;")] FlyNet,
    [IconText("&#xe64a;")] FlySettings,
    [IconText("&#xe645;")] Planet,
    [IconText("&#xe638;")] Settings,
    [IconText("&#xe65b;")] NotFound,
    [IconText("&#xe60c;")] Remove,
    [IconText("&#xe866;")] Save,
    [IconText("&#xe660;")] Add
}

[AttributeUsage(AttributeTargets.Field)]
internal sealed class IconTextAttribute(string text) : Attribute
{
    public string Text { get; set; } = text;
}

public static class IconTypeExtensions
{
    public static string GetIconText(this IconType iconType)
    {
        IconData.Current.TryGetValue(iconType.ToString(), out string? text);
        return text ?? string.Empty;
    }
}

public sealed class IconData
{
    public static ReadOnlyDictionary<string, string> Current { get; private set; }

    static IconData()
    {
        Dictionary<string, string> icons = [];
        var type = typeof(IconType);
        foreach (var member in type.GetMembers())
        {
            var key = member.Name;
            var val = member.GetCustomAttribute<IconTextAttribute>()?.Text ?? string.Empty;
            icons.TryAdd(key, val);
        }
        Current = new ReadOnlyDictionary<string, string>(icons);
    }
}