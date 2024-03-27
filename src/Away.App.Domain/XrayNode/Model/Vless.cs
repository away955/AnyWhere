using Away.App.Domain.XrayNode.Entities;

namespace Away.Domain.XrayNode.Model;

public sealed class Vless : IModelXrayNode
{
    public string password { get; set; } = string.Empty;
    public string host { get; set; } = string.Empty;
    public int port { get; set; }
    public string type { get; set; } = string.Empty;
    public string path { get; set; } = string.Empty;
    public string encryption { get; set; } = string.Empty;
    public string security { get; set; } = string.Empty;
    public string headerType { set; get; } = string.Empty;
    public string ps { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;

    public static Vless? Parse(string content)
    {
        try
        {
            var pattern = "^vless://(?<password>.*)@(?<host>.*):(?<port>\\d+)?(?<query>.*)#(?<ps>.*)";
            var reg = Regex.Match(content, pattern);
            if (!reg.Success)
            {
                return null;
            }

            var model = new Vless
            {
                url = content,
                host = reg.Result("${host}"),
                port = Convert.ToInt32(reg.Result("${port}")),
                password = reg.Result("${password}"),
                ps = XrayUtils.UrlDecode(reg.Result("${ps}"))
            };

            var query = reg.Result("${query}");
            var items = HttpUtility.ParseQueryString(query);
            if (items != null)
            {
                model.encryption = items.Get("encryption") ?? string.Empty;
                model.security = items.Get("security") ?? string.Empty;
                model.path = items.Get("path") ?? string.Empty;
                model.type = items.Get("type") ?? string.Empty;
                model.headerType = items.Get("headerType") ?? string.Empty;
            }

            model.url = content;
            return model;
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "vless解析错误：{content}", content);
            return null;
        }
    }

    public XrayNodeEntity ToEntity()
    {
        return new XrayNodeEntity
        {
            Url = url,
            Type = "vless",
            Alias = ps,
            Host = host,
            Port = port
        };
    }

    public XrayOutbound ToXrayOutbound()
    {
        var model = new XrayOutbound()
        {
            tag = "proxy",
            protocol = "vless",
        };

        // settings 配置
        var settings = new Dictionary<string, object>();
        var user = new
        {
            id = Guid.NewGuid().ToString(),
            encryption = encryption
        };
        var item = new
        {
            address = host,
            port = port,
            users = new[] { user }
        };
        settings.Add("vnext", new object[] { item });
        model.settings = settings;

        // streamSettings 配置
        model.streamSettings = new OutboundStreamSettings()
        {
            network = type
        };

        // mux 
        model.mux = new OutboundMux
        {
            enabled = false,
        };

        return model;
    }

}
