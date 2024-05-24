using Youtube.Services.Impl;
using Youtube.Views;

namespace Youtube;

/// <summary>
/// 油管视频插件注册
/// </summary>
public sealed class PluginRegister : PluginRegisterBase<PluginRegister>, IPluginRegister
{
    public string Module => "Youtube";

    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IYoutubeMapper, YoutubeMapper>();
        services.AddSingleton<IYoutubeFactory, YoutubeFactory>();
        services.AddSingleton<IYoutubeRepository, YoutubeRepository>();
        services.AddSingleton<IYoutubeService, YoutubeService>();

        // view model
        services.AddView<YoutubeView, YoutubeViewModel>("youtube");
        services.AddView<YoutubeAddView, YoutubeAddViewModel>("youtube-add");

        return base.ConfigureServices(services);
    }
}
