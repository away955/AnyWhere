using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Away.App.Core.Utils;
using System.Globalization;

namespace Away.App.Components.Converters;

/// <summary>
/// 从http加载图片
/// </summary>
public sealed class ImageUrlConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string url)
        {
            return null;
        }

        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        return AsyncUtils.RunSync(async () =>
        {
            using var http = HttpClientUtils.CreateHttpClient();
            var resp = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            if (!resp.IsSuccessStatusCode)
            {
                return null;
            }
            var bytes = await resp.Content.ReadAsByteArrayAsync();
            return new Bitmap(new MemoryStream(bytes));
        });
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
