using System.Text.RegularExpressions;
using System.Web;
using Away.Service.Utils;

namespace Away.Service.XrayNode.Model;

public class Trojan
{
    public string host { get; set; } = string.Empty;
    public int port { get; set; }
    public string password { get; set; } = string.Empty;
    public string security { get; set; } = string.Empty;
    public string sni { get; set; } = string.Empty;
    public string alpn { set; get; } = string.Empty;
    public string type { get; set; } = string.Empty;
    public string headerType { get; set; } = string.Empty;
    public string ps { get; set; } = string.Empty;

    public string url { get; set; } = string.Empty;


    public static Trojan? Parse(string content)
    {
        try
        {
            var pattern = "^trojan://(?<password>.*)@(?<host>.*):(?<port>.*)[?](?<query>.*)#(?<ps>.*)";
            var reg = Regex.Match(XrayUtils.UrlDecode(content), pattern);
            if (!reg.Success)
            {
                return null;
            }

            var trojan = new Trojan();
            trojan.host = reg.Result("${host}");
            trojan.port = Convert.ToInt32(reg.Result("${port}"));
            trojan.password = reg.Result("${password}");
            trojan.ps = reg.Result("${ps}");

            var query = reg.Result("${query}");
            var items = HttpUtility.ParseQueryString(query);
            //security=tls&sni=trojan.miwan.co.uk&alpn=http%2F1.1&type=tcp&headerType=none
            if (items != null)
            {
                trojan.security = items.Get("security");
                trojan.sni = items.Get("sni");
                trojan.alpn = items.Get("alpn");
                trojan.type = items.Get("type");
                trojan.headerType = items.Get("headerType");
            }


            return trojan;
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "trojan解析错误：{content}", content);
            return null;
        }
    }

    public XrayNodeEntity ToEntity()
    {
        return new XrayNodeEntity
        {
            Url = url,
            Type = "trojan",
            Alias = ps,
            Host = host,
            Port = port,
            security = security,
            Transport = type
        };
    }
}
