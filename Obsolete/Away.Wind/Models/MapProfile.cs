namespace Away.Wind.Models;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<XrayNodeEntity, XrayNodeModel>();
        CreateMap<XrayNodeModel, XrayNodeEntity>();
        CreateMap<XrayNodeSubModel, XrayNodeSubEntity>();
        CreateMap<XrayNodeSubEntity, XrayNodeSubModel>();

        CreateMap<XrayLogModel, XrayLog>();
        CreateMap<XrayLog, XrayLogModel>();

        CreateMap<XrayDnsModel, XrayDns>()
            .ForMember(s => s.servers, d => d.MapFrom(dd => dd.Items.Select(ddd => ddd.Item)));
        CreateMap<XrayDns, XrayDnsModel>()
            .ForMember(s => s.Items, d => d.MapFrom(dd => new ObservableCollection<StringItem>(dd.servers.Select(ddd => new StringItem(ddd)))));

        CreateMap<XrayInbound, XrayInboundModel>();
        CreateMap<XrayInboundModel, XrayInbound>();

        CreateMap<XrayNodeSubModel, XrayNodeSubEntity>();
        CreateMap<XrayNodeSubEntity, XrayNodeSubModel>();

        CreateMap<XrayRouteModel, XrayRoute>();
        CreateMap<XrayRoute, XrayRouteModel>();

        CreateMap<XrayRouteRuleModel, RouteRule>()
            .ForMember(s => s.domain, d => d.MapFrom(dd => dd.Domain.Split(";", StringSplitOptions.RemoveEmptyEntries)))
            .ForMember(s => s.ip, d => d.MapFrom(dd => dd.IP.Split(";", StringSplitOptions.RemoveEmptyEntries)))
            .ForMember(s => s.inboundTag, d => d.MapFrom(dd => dd.InboundTag.Split(";", StringSplitOptions.RemoveEmptyEntries)));
        CreateMap<RouteRule, XrayRouteRuleModel>()
             .ForMember(s => s.Domain, d => d.MapFrom(dd => string.Join(";", dd.domain ?? new List<string>())))
             .ForMember(s => s.IP, d => d.MapFrom(dd => string.Join(";", dd.ip ?? new List<string>())))
             .ForMember(s => s.InboundTag, d => d.MapFrom(dd => string.Join(";", dd.inboundTag ?? new List<string>())));


    }
}

