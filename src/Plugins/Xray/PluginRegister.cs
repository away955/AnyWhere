using Away.App.Core.Extensions.DependencyInjection;
using Xray.Services.Impl;
using Xray.Views;

namespace Xray;

/// <summary>
/// 网络代理插件注册
/// </summary>
public sealed class PluginRegister : PluginRegisterBase<PluginRegister>, IPluginRegister
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSqlSugarClient(Constant.DBConn, Constant.DBKey);
        services.AddSingleton<IXraySetting, XraySettings>();
        services.AddSingleton<IMapper, MapProfile>();
        services.AddSingleton<IXrayNodeRepository, XrayNodeRepository>();
        services.AddSingleton<IXrayNodeService, XrayNodeService>();
        services.AddSingleton<IXrayNodeSubRepository, XrayNodeSubRepository>();
        services.AddSingleton<IXrayNodeSubService, XrayNodeSubService>();
        services.AddSingleton<IXrayService, XrayService>();
        services.AddSingleton<IXraySettingService, XraySettingService>();


        if (OperatingSystem.IsWindows())
        {
            services.AddScoped<IProxySetting, WindowsProxySetting>();
        }
        if (OperatingSystem.IsMacOS())
        {
            services.AddScoped<IProxySetting, MacOSProxySetting>();
        }
        if (OperatingSystem.IsLinux())
        {
            services.AddScoped<IProxySetting, LinuxProxySetting>();
        }

        // view model
        services.AddView<XrayNodesView, XrayNodesViewModel>("xray");
        services.AddView<XraySettingsView>("xray-settings");
        services.AddView<XrayNodeSubView, XrayNodeSubViewModel>("xray-setting-sub");
        services.AddView<XrayTestSettingsView, XrayTestSettingsViewModel>("xray-setting-test");
        services.AddView<XrayDnsView, XrayDnsViewModel>("xray-setting-dns");
        services.AddView<XrayInboundView, XrayInboundViewModel>("xray-setting-inbound");
        services.AddView<XrayOutboundView, XrayOutboundViewModel>("xray-setting-outbound");
        services.AddView<XrayLogView, XrayLogViewModel>("xray-setting-log");
        services.AddView<XrayRouteView, XrayRouteViewModel>("xray-setting-route");

    }

    public override void ApplicationExit()
    {
        var serivce = AwayLocator.GetService<IXrayService>();
        serivce?.CloseAll();
    }
}