using Away.App.Core.Extensions.DependencyInjection;
using Youtube.Services.Impl;
using Youtube.Views;

namespace Youtube;

/// <summary>
/// 油管视频插件注册
/// </summary>
public sealed class PluginRegister : PluginRegisterBase<PluginRegister>, IPluginRegister
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSqlSugarClient(Constant.DBConn, Constant.DBKey);
        services.AddSingleton<IMapper, MapProfile>();
        services.AddSingleton<IYoutubeFactory, YoutubeFactory>();
        services.AddSingleton<IYoutubeRepository, YoutubeRepository>();
        services.AddSingleton<IYoutubeService, YoutubeService>();

        // view model
        services.AddView<YoutubeView, YoutubeViewModel>("youtube");
        services.AddView<YoutubeAddView, YoutubeAddViewModel>("youtube-add");
    }
}
