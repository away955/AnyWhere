using AutoMapper;
using Away.Service.DB.Entities;

namespace Away.Wind.Views.Xray.Models;

public class MapProfile : Profile
{
    public MapProfile()
    {
        this.CreateMap<XrayNodeEntity, XrayNodeModel>();
        this.CreateMap<XrayNodeModel, XrayNodeEntity>();
    }
}
