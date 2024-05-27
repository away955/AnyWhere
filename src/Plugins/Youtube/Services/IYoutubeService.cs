namespace Youtube.Services;

public interface IYoutubeService
{
    string GetFolderPath(YoutubeEntity entity);
    bool Remove(YoutubeEntity entity);
    bool Save(YoutubeEntity entity);
    List<YoutubeEntity> GetList();
    YoutubeEntity GetById(int id);
}
