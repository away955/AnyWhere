using RouterScanner.Services;
using RouterScanner.Services.Impl;
using RouterScanner.Services.VulHub;
using RouterScanner.Views;

namespace RouterScanner;

/// <summary>
/// 路由器漏洞扫描插件注册
/// </summary>
public sealed class PluginRegister : PluginRegisterBase<PluginRegister>, IPluginRegister
{
    public string Module => "RouterScanner";

    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IRouterScanner, Services.Impl.RouterScanner>();
        services.AddTransient<IRouterFingerPrintHub, RouterFingerPrintHub>();
        services.AddTransient<IRouterFingerPrintScanner, RouterFingerPrintScanner>();
        services.AddTransient<IRouterVulScanner, RouterVulScanner>();

        // 视图
        services.AddView<RouterView, RouterViewModel>("router");
        services.AddView<RouterExploitView, RouterExploitViewModel>("router-exp");

        // 2018 vulhub
        services.AddKeyedScoped<IRouterVulHub, CVE_2018_12692>(Constant.VulHubKey);
        // 2019 vulhub
        services.AddKeyedScoped<IRouterVulHub, CVE_2019_16893>(Constant.VulHubKey);
        services.AddKeyedScoped<IRouterVulHub, CVE_2019_6971>(Constant.VulHubKey);
        // 2020 vulhub
        services.AddKeyedScoped<IRouterVulHub, CVE_2020_24363>(Constant.VulHubKey);
        services.AddKeyedScoped<IRouterVulHub, CVE_2020_35576>(Constant.VulHubKey);
        services.AddKeyedScoped<IRouterVulHub, CVE_2020_9375>(Constant.VulHubKey);
        // 2021 vulhub
        services.AddKeyedScoped<IRouterVulHub, CVE_2021_4045>(Constant.VulHubKey);
        // 2023 vulhub
        services.AddKeyedScoped<IRouterVulHub, CVE_2023_1389>(Constant.VulHubKey);
        services.AddKeyedScoped<IRouterVulHub, CVE_2023_36355>(Constant.VulHubKey);

        return base.ConfigureServices(services);
    }
}
