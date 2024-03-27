using YoutubeExplode.Videos;

namespace Away.App.Domain.Youtube;

public interface IYoutubeService
{
    ValueTask<Video> GetVideoInfo(string url);
}
