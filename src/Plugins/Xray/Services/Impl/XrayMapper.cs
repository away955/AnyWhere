namespace Xray.Services.Impl;

public sealed class XrayMapper : Mapper, IXrayMapper
{
    public XrayMapper()
    {
        Config.ForType<XrayTestSettingsModel, SpeedTestSettings>();
        Config.ForType<SpeedTestSettings, XrayTestSettingsModel>();
        Config.ForType<XrayNodeEntity, XrayNodeModel>();
        Config.ForType<XrayNodeModel, XrayNodeEntity>();
        Config.ForType<XrayNodeSubModel, XrayNodeSubEntity>();
        Config.ForType<XrayNodeSubEntity, XrayNodeSubModel>();

        Config.ForType<XrayLogModel, XrayLog>();
        Config.ForType<XrayLog, XrayLogModel>();

        Config.ForType<XrayDnsModel, XrayDns>()
            .Map(d => d.servers, s => s.Items.Select(o => o.Item));
        Config.ForType<XrayDns, XrayDnsModel>()
            .Map(d => d.Items, s => new ObservableCollection<StringItem>(s.servers.Select(o => new StringItem(o))));

        Config.ForType<XrayInbound, XrayInboundModel>();
        Config.ForType<XrayInboundModel, XrayInbound>();

        Config.ForType<XrayNodeSubModel, XrayNodeSubEntity>();
        Config.ForType<XrayNodeSubEntity, XrayNodeSubModel>();

        Config.ForType<XrayRouteModel, XrayRoute>();
        Config.ForType<XrayRoute, XrayRouteModel>();

        Config.ForType<XrayRouteRuleModel, RouteRule>()
            .Map(d => d.domain, s => s.DomainStr.Split(";", StringSplitOptions.RemoveEmptyEntries))
            .Map(d => d.ip, s => s.IPStr.Split(";", StringSplitOptions.RemoveEmptyEntries))
            .Map(d => d.inboundTag, s => s.InboundTagStr.Split(";", StringSplitOptions.RemoveEmptyEntries));
        Config.ForType<RouteRule, XrayRouteRuleModel>()
             .Map(d => d.DomainStr, s => string.Join(";", s.domain ?? new List<string>()))
             .Map(d => d.IPStr, s => string.Join(";", s.ip ?? new List<string>()))
             .Map(d => d.InboundTagStr, s => string.Join(";", s.inboundTag ?? new List<string>()));

        Config.ForType<XrayOutbound, XrayOutboundModel>()
            .Map(d => d.SettingStr, s => s.settings == null ? string.Empty : JsonUtils.Serialize(s.settings));
    }
}
