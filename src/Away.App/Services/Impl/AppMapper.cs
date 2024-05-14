namespace Away.App.Services.Impl;

public sealed class AppMapper : Mapper, IAppMapper
{
    public AppMapper() : base(new())
    {
        Config.ForType<AppMenuEntity, AppMenuModel>();

        Config.ForType<PluginStoreModel, PluginRegisterEntity>()
            .Map(d => d.Version, s => s.LatestVersion);

        Config.ForType<PluginStoreModel, PluginModel>();
        Config.ForType<PluginModel, PluginStoreModel>();
    }
}
