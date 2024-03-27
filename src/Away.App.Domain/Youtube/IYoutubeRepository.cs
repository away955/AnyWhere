using Away.App.Domain.Youtube.Entities;

namespace Away.App.Domain.Youtube;

public interface IYoutubeRepository
{
    YoutubeSettingsEntity GetSetting(string key);
}
