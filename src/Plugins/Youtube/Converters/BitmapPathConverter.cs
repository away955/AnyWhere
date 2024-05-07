using Avalonia.Data.Converters;
using System.Globalization;

namespace Youtube.Converters;

public sealed class BitmapPathConverter : IValueConverter
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
