namespace Away.Domain.XrayNode;


public static class XrayExtentions
{
    public static void RemoveInbound(this XrayConfig config, string tag)
    {
        var index = config.inbounds.FindIndex(o => o.tag == tag);
        if (index > -1)
        {
            config.inbounds.RemoveAt(index);
        }
    }

    public static void SetInbound(this XrayConfig config, string tag, string protocol, int port)
    {
        XrayInbound inbound = new()
        {
            tag = tag,
            protocol = protocol,
            port = port
        };
        config.SetInbound(inbound);
    }

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

    public static bool SetOutbound(this XrayConfig config, XrayNodeEntity entity)
    {
        if ("vmess" == entity.Type)
        {
            var model = Vmess.Parse(entity.Url);
            if (model == null)
            {
                return false;
            }
            config.SetOutbound(model);
        }
        else if ("vless" == entity.Type)
        {
            var model = Vless.Parse(entity.Url);
            if (model == null)
            {
                return false;
            }
            config.SetOutbound(model);
        }
        else if ("trojan" == entity.Type)
        {
            var model = Trojan.Parse(entity.Url);
            if (model == null)
            {
                return false;
            }
            config.SetOutbound(model);
        }
        else if ("ss" == entity.Type)
        {
            var model = Shadowsocks.Parse(entity.Url);
            if (model == null)
            {
                return false;
            }
            config.SetOutbound(model);
        }
        else if ("ssr" == entity.Type)
        {
            var model = ShadowsocksR.Parse(entity.Url);
            if (model == null)
            {
                return false;
            }
            config.SetOutbound(model);
        }
        return true;
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
