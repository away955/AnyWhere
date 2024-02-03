using Away.Service.XrayNode;

namespace Away.Service.Xray;



public static class XrayExtentions
{
    public static void SetOutbound<T>(this XrayConfig config, T model) where T : IModelXrayNode
    {
        config.outbounds ??= [];
        var outbound = model.ToXrayOutbound();

        var index = config.outbounds.FindIndex(o => o.tag == outbound.tag);
        if (index > -1)
        {
            config.outbounds.RemoveAt(index);
        }
        config.outbounds.Add(outbound);
    }
}
