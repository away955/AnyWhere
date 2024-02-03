using System.Text.RegularExpressions;
using System.Web;
using Away.Service.Utils;
using Away.Service.Xray.Model;

namespace Away.Service.XrayNode.Model;

public class Trojan : IModelXrayNode
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
            if (items != null)
            {
                trojan.security = items.Get("security") ?? string.Empty;
                trojan.sni = items.Get("sni") ?? string.Empty;
                trojan.alpn = items.Get("alpn") ?? string.Empty;
                trojan.type = items.Get("type") ?? string.Empty;
                trojan.headerType = items.Get("headerType") ?? string.Empty;
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
        };
    }

    public XrayOutbound ToXrayOutbound()
    {
        var model = new XrayOutbound()
        {
            tag = "proxy",
            protocol = "trojan",
        };

        // settings 配置
        var settings = new Dictionary<string, object>();
        var item = new
        {
            address = host,
            method = "chacha20",
            ota = false,
            password = password,
            port = port,
            level = 1
        };
        settings.Add("servers", new object[] { item });
        model.settings = settings;

        // streamSettings 配置
        model.streamSettings = new OutboundStreamSettings()
        {
            network = type,
            security = security,
            tcpSettings = new
            {
                allowInsecure = false,
                serverName = sni,
                alpn = new List<string> { alpn },
                show = false
            }
        };

        // mux 
        model.mux = new OutboundMux
        {
            enabled = false,
        };

        return model;
    }
}
