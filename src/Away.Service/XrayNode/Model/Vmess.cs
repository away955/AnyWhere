using System.Text.Json;
using System.Text.RegularExpressions;
using Away.Service.Utils;

namespace Away.Service.XrayNode.Model;

public class Vmess
{
    public string v { get; set; } = string.Empty;
    public string ps { get; set; } = string.Empty;
    public int port { get; set; }
    public string id { get; set; } = string.Empty;
    public int aid { get; set; }
    public string net { get; set; } = string.Empty;
    public string type { get; set; } = string.Empty;
    public string host { get; set; } = string.Empty;
    public string add { get; set; } = string.Empty;
    public string path { get; set; } = string.Empty;
    public string tls { get; set; } = string.Empty;
    public string scy { get; set; } = string.Empty;
    public string sni { get; set; } = string.Empty;
    public string alpn { set; get; } = string.Empty;
    public string fp { get; set; } = string.Empty;

    public string url { get; set; } = string.Empty;

    public static Vmess? Parse(string content)
    {
        try
        {
            var pattern = "^vmess://(?<text>.*)";
            var reg = Regex.Match(content, pattern);
            if (!reg.Success)
            {
                return null;
            }

            var text = XrayUtils.Base64Decode(reg.Result("${text}"));
            var model = XrayUtils.Deserialize<Vmess>(text);
            if (model == null)
            {
                return null;
            }
            model.url = content;
            return model;
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "vmess解析错误：{content}", content);
            return null;
        }
    }

    public XrayNodeEntity ToEntity()
    {
        return new XrayNodeEntity
        {
            Url = url,
            Type = "vmess",
            Alias = ps,
            Host = add,
            Port = port,
            security = scy,
            Transport = net,
            TLS = tls
        };
    }
}
