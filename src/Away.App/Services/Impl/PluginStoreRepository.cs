namespace Away.App.Services.Impl;

public sealed class PluginStoreRepository : IPluginStoreRepository
{
    private readonly SimpleClient<PluginRegisterEntity> _pluginRegister;
    private readonly SimpleClient<PluginStoreEntity> _pluginStore;

    public PluginStoreRepository(ISugarDbContext db)
    {
        db.CodeFirst.InitTables<PluginRegisterEntity, PluginStoreEntity>();
        _pluginRegister = db.GetSimpleClient<PluginRegisterEntity>();
        _pluginStore = db.GetSimpleClient<PluginStoreEntity>();
    }

    public List<PluginRegisterEntity> GetPluginRegisters()
    {
        return _pluginRegister.AsQueryable().Where(o => o.IsDisabled == false).ToList();
    }

    public bool Register(PluginRegisterEntity entity)
    {
        return _pluginRegister.InsertOrUpdate(entity);
    }

    public bool UnRegister(string module)
    {
        return _pluginRegister.DeleteById(module);
    }

    public List<PluginStoreModel> GetList()
    {
        return _pluginStore.AsQueryable()
              .LeftJoin<PluginRegisterEntity>((o, r) => o.Module == r.Module)
              .Select((o, r) => new PluginStoreModel
              {
                  Module = o.Module,
                  Name = o.Name,
                  Description = o.Description,
                  ContentID = o.ContentID,
                  LatestVersion = o.Version,
                  IsDisabled = r.IsDisabled,
                  CurrentVersion = r.Version,
                  Logo = o.Logo,
                  FileSize = o.FileSize,
                  Path = o.Path,
                  Image = o.Image
              })
              .ToList();
    }

    public bool Save(List<PluginStoreEntity> list)
    {
        return _pluginStore.InsertOrUpdate(list);
    }
}
