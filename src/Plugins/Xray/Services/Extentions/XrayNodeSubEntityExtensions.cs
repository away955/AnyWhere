namespace Xray.Services.Extentions;

public static class XrayNodeSubEntityExtensions
{
    public static string ParseUrl(this XrayNodeSubEntity entity)
    {
        var (_, url) = ParseDateTime(entity.Url);
        return url;
    }

    private static (bool, string) ParseDateTime(string url)
    {
        var pattern = @"\${(?<date>\w+):(?<format>[-/\w]+)}";
        var reg = Regex.Match(url, pattern);
        if (!reg.Success)
        {
            return (false, url);
        }
        var format = reg.Result("${format}");
        var date = DateTime.Now.ToString(format);
        var newUrl = url.Replace(reg.Value, date);
        return ParseDateTime(newUrl);
    }
}
