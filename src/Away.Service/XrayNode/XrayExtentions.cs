namespace Away.Service.XrayNode;


public static class XrayExtentions
{
    public static void SetInbound(this XrayConfig config, XrayInbound model)
    {
        config.inbounds ??= [];

        var index = config.inbounds.FindIndex(o => o.tag == model.tag);
        if (index > -1)
        {
            config.inbounds.RemoveAt(index);
        }
        config.inbounds.Insert(0, model);
    }

    public static void SetOutbound(this XrayConfig config, XrayNodeEntity entity)
    {
        if ("vmess" == entity.Type)
        {
            var model = Vmess.Parse(entity.Url);
            config.SetOutbound(model!);
        }
        else if ("trojan" == entity.Type)
        {
            var model = Trojan.Parse(entity.Url);
            config.SetOutbound(model!);
        }
        else if ("shadowsocks" == entity.Type)
        {
            var model = Shadowsocks.Parse(entity.Url);
            config.SetOutbound(model!);
        }
    }

    public static void SetOutbound<T>(this XrayConfig config, T model) where T : IModelXrayNode
    {
        config.outbounds ??= [];
        var outbound = model.ToXrayOutbound();

        var index = config.outbounds.FindIndex(o => o.tag == outbound.tag);
        if (index > -1)
        {
            config.outbounds.RemoveAt(index);
        }
        config.outbounds.Insert(0, outbound);
    }
}
