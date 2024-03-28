using AngleSharp.Common;
using Avalonia.Data.Converters;
using System.Globalization;

namespace Away.App.Converters;

public sealed class RowConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var dic = value.ToDictionary();
        if (dic == null)
        {
            return null;
        }
        if (!dic.TryGetValue("IsChecked", out string? isChekced))
        {
            return null;
        }
        if (!System.Convert.ToBoolean(isChekced))
        {
            return null;
        }
        return Thickness.Parse(System.Convert.ToString(parameter) ?? "0");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}

public sealed class CheckedModel
{
    public bool IsChekced { get; set; }
}
