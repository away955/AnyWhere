namespace Away.Domain.XrayNode.Model;

public sealed class Shadowsocks : IModelXrayNode
{
    public string ps { get; set; } = string.Empty;
    public int port { get; set; }
    public string host { get; set; } = string.Empty;
    public string method { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;

    public static Shadowsocks? Parse(string content)
    {
        try
        {
            var pattern = "^ss://(?<password>.*)@(?<host>.*):(?<port>\\d+)(?<query>.*)#(?<ps>.*)";
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
            var reg_passwd = Regex.Match(passwd, "(?<method>.*):(?<password>.*)");
            if (reg_passwd.Success)
            {
                model.method = reg_passwd.Result("${method}");
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
            Type = "ss",
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
            protocol = "shadowsocks",
        };

        // settings 配置
        var settings = new Dictionary<string, object>();
        var item = new
        {
            address = host,
            port = port,
            method = method,
            ota = false,
            password = password,
            level = 1
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
