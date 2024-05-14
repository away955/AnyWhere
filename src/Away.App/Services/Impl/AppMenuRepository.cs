namespace Away.App.Services.Impl;

public sealed class AppMenuRepository : IAppMenuRepository
{
    private readonly SimpleClient<AppMenuEntity> _menu;

    public AppMenuRepository([FromKeyedServices(Constant.DBKey)] ISugarDbContext db)
    {
        db.CodeFirst.InitTables<AppMenuEntity>();
        _menu = db.GetSimpleClient<AppMenuEntity>();
        SeedData();
    }

    private void SeedData()
    {
        var entity = new AppMenuEntity
        {
            Module = "Away.App",
            Path = "app-store",
            Title = "插件商店",
            Icon = "&#xeba9;",
            SortOrder = 1,
            IsDisabled = false
        };
        if (_menu.IsAny(o => o.Module == entity.Module))
        {
            return;
        }
        Save(entity);
    }

    public List<AppMenuEntity> GetList()
    {
        return _menu.AsQueryable().Where(o => o.IsDisabled == false).OrderBy(o => o.SortOrder).ToList();
    }

    public bool Save(AppMenuEntity entity)
    {
        return _menu.InsertOrUpdate(entity);
    }

    public bool DeleteByModule(string module)
    {
        return _menu.Delete(o => o.Module == module);
    }

    public bool IsDisabledByModule(string module)
    {
        return _menu.Update(o => new AppMenuEntity { IsDisabled = !o.IsDisabled }, t => t.Module == module);
    }
}
