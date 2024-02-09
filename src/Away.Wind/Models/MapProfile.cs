namespace Away.Wind.Models;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<XrayNodeEntity, XrayNodeModel>();
        CreateMap<XrayNodeModel, XrayNodeEntity>();

        CreateMap<XrayLogModel, XrayLog>();
        CreateMap<XrayLog, XrayLogModel>();

        CreateMap<XrayInbound, XrayInboundModel>();

        CreateMap<XrayNodeSubModel, XrayNodeSubEntity>();
        CreateMap<XrayNodeSubEntity, XrayNodeSubModel>();
    }
}
