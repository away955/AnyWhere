using Away.App.Domain.Xray.Entities;

namespace Away.App.Domain.Xray.Models;

public sealed class Trojan : IModelXrayNode
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
            var pattern = @"^trojan://(?<password>.*)@(?<host>.*):(?<port>\d+)(?<query>.*)#(?<ps>.*)";
            var reg = Regex.Match(XrayUtils.UrlDecode(content), pattern);
            if (!reg.Success)
            {
                return null;
            }

            var trojan = new Trojan
            {
                url = content,
                host = reg.Result("${host}"),
                port = Convert.ToInt32(reg.Result("${port}")),
                password = reg.Result("${password}"),
                ps = reg.Result("${ps}")
            };

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
            Log.Error(ex, "trojan解析错误：{content}", content);
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
            password,
            port,
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
