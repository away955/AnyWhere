namespace Youtube.Services.Impl;

public sealed class YoutubeRepository : IYoutubeRepository
{
    private readonly ISugarDbContext db;
    public YoutubeRepository([FromKeyedServices(Constant.DBKey)] ISugarDbContext db)
    {
        this.db = db;
        db.CodeFirst.InitTables<YoutubeEntity>();
    }

    private ISimpleClient<YoutubeEntity>? _youtubeTb;

    private ISimpleClient<YoutubeEntity> YoutubeTb => _youtubeTb ??= db.GetSimpleClient<YoutubeEntity>();


    public List<YoutubeEntity> GetList()
    {
        return YoutubeTb.GetList();
    }
    public bool DeleteById(int id)
    {
        return YoutubeTb.DeleteById(id);
    }

    public YoutubeEntity GetById(int id)
    {
        return YoutubeTb.GetById(id);
    }

    public bool InsertOrUpdate(YoutubeEntity entity)
    {
        return db.Storageable(entity).WhereColumns(o => o.Source).ExecuteCommand() > 0;
    }

    public bool SetState(int id, YoutubeVideoState state)
    {
        return YoutubeTb.AsUpdateable()
             .SetColumns(o => new YoutubeEntity { State = state })
             .Where(o => o.Id == id)
             .ExecuteCommand() > 0;
    }
}
