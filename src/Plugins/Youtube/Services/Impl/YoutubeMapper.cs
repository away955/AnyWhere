namespace Youtube.Services.Impl;

public class YoutubeMapper : Mapper, IYoutubeMapper
{
    public YoutubeMapper()
    {
        Config.ForType<YoutubeEntity, YoutubeModel>();
    }
}
