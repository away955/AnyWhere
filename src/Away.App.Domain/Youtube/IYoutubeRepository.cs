using Away.App.Domain.Youtube.Entities;

namespace Away.App.Domain.Youtube;

public interface IYoutubeRepository
{
    bool SetState(int id, YoutubeVideoState state);
    YoutubeEntity GetById(int id);
    bool InsertOrUpdate(YoutubeEntity entity);
    List<YoutubeEntity> GetList();
    bool DeleteById(int id);


}
