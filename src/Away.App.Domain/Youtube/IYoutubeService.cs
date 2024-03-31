using Away.App.Domain.Youtube.Entities;

namespace Away.App.Domain.Youtube;

public interface IYoutubeService
{
    string GetFolderPath(YoutubeEntity entity);
    bool Remove(YoutubeEntity entity);
    bool Save(YoutubeEntity entity);
    List<YoutubeEntity> GetList();
}
