using Away.App.Domain.Youtube.Entities;

namespace Away.App.Domain.Youtube;

public interface IYoutubeRepository
{
    List<YoutubeEntity> GetList();
    bool DeleteById(int id);

    YoutubeSettingsEntity GetSetting(string key);

}
