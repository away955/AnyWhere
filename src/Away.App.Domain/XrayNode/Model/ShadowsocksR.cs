namespace Away.Domain.XrayNode.Model;

public sealed class ShadowsocksR : IModelXrayNode
{
    public string server { get; set; } = string.Empty;
    public int port { get; set; }
    public string protocol { get; set; } = string.Empty;
    public string method { get; set; } = string.Empty;
    public string obfs { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string obfsparam { get; set; } = string.Empty;
    public string protoparam { get; set; } = string.Empty;
    public string ps { get; set; } = string.Empty;
    public string group { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;

    public static ShadowsocksR? Parse(string content)
    {
        try
        {
            if (!content.StartsWith("ssr://"))
            {
                return null;
            }
            var url = content.Replace("ssr://", string.Empty).Replace("-", "+").Replace("_", "/");
            var str = XrayUtils.Base64Decode(url);
            var pattern = "(?<server>.*):(?<port>\\d+):(?<protocol>.*):(?<method>.*):(?<obfs>.*):(?<password_base64>.*)/(?<query>.*)";
            var reg = Regex.Match(str, pattern);
            if (!reg.Success)
            {
                return null;
            }
            var model = new ShadowsocksR
            {
                url = content,
                server = reg.Result("${server}"),
                port = Convert.ToInt32(reg.Result("${port}")),
                protocol = reg.Result("${protocol}"),
                method = reg.Result("${method}"),
                obfs = reg.Result("${obfs}"),
                password = XrayUtils.Base64Decode(reg.Result("${password_base64}")),
            };
            var query = reg.Result("${query}");
            var items = HttpUtility.ParseQueryString(query);
            if (items != null)
            {
                model.obfsparam = XrayUtils.Base64Decode(items.Get("obfsparam")!);
                model.protoparam = XrayUtils.Base64Decode(items.Get("protoparam")!);
                model.ps = XrayUtils.Base64Decode(items.Get("remarks")!);
                model.group = XrayUtils.Base64Decode(items.Get("group")!);
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
            Type = "ssr",
            Alias = ps,
            Host = server,
            Port = port
        };
    }

    public XrayOutbound ToXrayOutbound()
    {
        var model = new XrayOutbound()
        {
            tag = "proxy",
            protocol = "shadowsocks",
        };

        // settings 配置
        var settings = new Dictionary<string, object>();
        var item = new
        {
            address = server,
            port = port,
            method = method,
            password = password,
        };
        settings.Add("servers", new object[] { item });
        model.settings = settings;

        // streamSettings 配置
        model.streamSettings = new OutboundStreamSettings()
        {
            network = "tcp"
        };

        // mux 
        model.mux = new OutboundMux
        {
            enabled = false,
        };

        return model;
    }
}
