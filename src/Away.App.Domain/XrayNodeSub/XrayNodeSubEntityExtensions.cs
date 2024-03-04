using System.Text.RegularExpressions;

namespace Away.App.Domain.XrayNodeSub;

public static class XrayNodeSubEntityExtensions
{
    public static string ParseUrl(this XrayNodeSubEntity entity)
    {
        var pattern = "[$]{(?<date>.*):(?<format>.*)}";
        var reg = Regex.Match(entity.Url, pattern);
        if (!reg.Success)
        {
            return entity.Url;
        }
        var format = reg.Result("${format}");
        var date = DateTime.Now.ToString(format);
        return Regex.Replace(entity.Url, "[$]{.*}", date);

    }
}
