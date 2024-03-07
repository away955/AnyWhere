namespace Away.Domain.XrayNode.Impl;

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

            var shadowsocks = Shadowsocks.Parse(item);
            if (shadowsocks != null)
            {
                list.Add(shadowsocks.ToEntity());
                continue;
            }

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
        var entities = list.Where(o => !o.Alias.StartsWith("更新于"))
            .Select(o =>
            {
                var context = o.Alias.Split('-');
                if (context.Length > 1)
                {
                    o.Alias = context.LastOrDefault()?.Trim() ?? string.Empty;
                }
                return o;
            }).ToList();
        _xrayNodeRepository.SaveNodes(entities);
        Log.Information($"更新{entities.Count}个节点");
    }
}
