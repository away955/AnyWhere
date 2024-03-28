using Away.App.Domain.Youtube.Entities;

namespace Away.App.Domain.Youtube;

public interface IYoutubeRepository
{
    List<YoutubeEntity> GetList();
    YoutubeSettingsEntity GetSetting(string key);

}
