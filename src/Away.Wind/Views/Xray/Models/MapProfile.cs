namespace Away.Wind.Views.Xray.Models;

public class MapProfile : Profile
{
    public MapProfile()
    {
        this.CreateMap<XrayNodeEntity, XrayNodeModel>();
        this.CreateMap<XrayNodeModel, XrayNodeEntity>();

        CreateMap<XrayLogModel, XrayLog>();
        CreateMap<XrayLog, XrayLogModel>();
    }
}
