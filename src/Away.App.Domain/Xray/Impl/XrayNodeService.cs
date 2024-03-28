using Away.App.Domain.Xray.Entities;
using Away.App.Domain.Xray.Models;

namespace Away.App.Domain.Xray.Impl;

[DI(ServiceLifetime.Singleton)]
public class XrayNodeService(IXrayNodeRepository xrayNodeRepository) : IXrayNodeService
{
    private readonly IXrayNodeRepository _xrayNodeRepository = xrayNodeRepository;

    public void SaveNodes(List<string> nodes)
    {
        var unknows = new HashSet<string>();
        var list = new List<XrayNodeEntity>();
        foreach (var item in nodes)
        {
            var vmess = Vmess.Parse(item);
            if (vmess != null)
            {
                list.Add(vmess.ToEntity());
                continue;
            }

            var vless = Vless.Parse(item);
            if (vless != null)
            {
                list.Add(vless.ToEntity());
                continue;
            }

            var shadowsocks = Shadowsocks.Parse(item);
            if (shadowsocks != null)
            {
                list.Add(shadowsocks.ToEntity());
                continue;
            }

            //var shadowsocksR = ShadowsocksR.Parse(item);
            //if (shadowsocksR != null)
            //{
            //    list.Add(shadowsocksR.ToEntity());
            //    continue;
            //}

            var trojan = Trojan.Parse(item);
            if (trojan != null)
            {
                list.Add(trojan.ToEntity());
                continue;
            }

            unknows.Add(item);
        }
        if (unknows.Count > 0)
        {
            Log.Warning($"未知类型\n\r{JsonUtils.Serialize(unknows.ToArray())}");
        }
        var items = list.DistinctBy(o => new { o.Host, o.Port }).ToList();
        _xrayNodeRepository.SaveNodes(items);
        Log.Information($"更新{list.Count}个节点");
    }
}
