using System.Text.RegularExpressions;
using Away.Service.Utils;

namespace Away.Service.XrayNode.Model;

public class Shadowsocks
{
    public string ps { get; set; } = string.Empty;
    public int port { get; set; }
    public string host { get; set; } = string.Empty;
    public string scy { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;

    public static Shadowsocks? Parse(string content)
    {
        try
        {
            var pattern = "^ss://(?<password>.*)@(?<host>.*):(?<port>.*)#(?<ps>.*)";
            var reg = Regex.Match(content, pattern);
            if (!reg.Success)
            {
                return null;
            }
            var model = new Shadowsocks
            {
                url = content,
                host = reg.Result("${host}"),
                port = Convert.ToInt32(reg.Result("${port}")),
                ps = XrayUtils.UrlDecode(reg.Result("${ps}"))
            };


            var passwd = XrayUtils.Base64Decode(reg.Result("${password}"));
            var reg_passwd = Regex.Match(passwd, "(?<username>.*):(?<password>.*)");
            if (reg_passwd.Success)
            {
                model.scy = reg_passwd.Result("${username}");
                model.password = reg_passwd.Result("${password}");
            }

            return model;
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "shadowsocks解析错误：{content}", content);
            return null;
        }
    }

    public XrayNodeEntity ToEntity()
    {
        return new XrayNodeEntity
        {
            Url = url,
            Type = "shadowsocks",
            Alias = ps,
            Host = host,
            Port = port,
            security = scy,
        };
    }
}
