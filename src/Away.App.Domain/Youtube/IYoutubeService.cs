using Away.App.Domain.Youtube.Entities;
using YoutubeExplode.Videos;

namespace Away.App.Domain.Youtube;

public interface IYoutubeService
{
    List<YoutubeEntity> GetList();
    ValueTask<Video> GetVideoInfo(string url);
}
