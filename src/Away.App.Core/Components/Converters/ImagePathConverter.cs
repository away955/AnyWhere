using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System.Globalization;

namespace Away.App.Components.Converters;

/// <summary>
/// 从本地磁盘加载图片
/// </summary>
public sealed class ImagePathConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string path)
        {
            return null;
        }
        if (!File.Exists(path))
        {
            return null;
        }
        var buffer = File.ReadAllBytes(path);
        return new Bitmap(new MemoryStream(buffer));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
