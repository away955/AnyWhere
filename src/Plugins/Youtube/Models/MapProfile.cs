namespace Youtube.Models;

public sealed class MapProfile : Mapper, IMapper
{
    public MapProfile()
    {
        Config.ForType<YoutubeEntity, YoutubeModel>();
    }
}

