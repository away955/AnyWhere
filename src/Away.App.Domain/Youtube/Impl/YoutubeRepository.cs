using Away.App.Domain.Youtube.Entities;

namespace Away.App.Domain.Youtube.Impl;

[DI]
internal sealed class YoutubeRepository : IYoutubeRepository
{
    private readonly ISugarDbContext db;
    public YoutubeRepository(ISugarDbContext db)
    {
        this.db = db;
        db.CodeFirst.InitTables<YoutubeEntity, YoutubeSettingsEntity>();
    }

    private ISimpleClient<YoutubeEntity>? _youtubeTb;
    private ISimpleClient<YoutubeSettingsEntity>? _settings;

    private ISimpleClient<YoutubeEntity> YoutubeTb => _youtubeTb ??= db.GetSimpleClient<YoutubeEntity>();
    private ISimpleClient<YoutubeSettingsEntity> Settings => _settings ??= db.GetSimpleClient<YoutubeSettingsEntity>();


    public List<YoutubeEntity> GetList()
    {
        return YoutubeTb.GetList();
    }
    public bool DeleteById(int id)
    {
        return YoutubeTb.DeleteById(id);
    }

    public List<YoutubeSettingsEntity> GetSettings()
    {
        return Settings.GetList();
    }

    public YoutubeSettingsEntity GetSetting(string key)
    {
        return Settings.AsQueryable().Where(o => o.Key == key).First();
    }

}
