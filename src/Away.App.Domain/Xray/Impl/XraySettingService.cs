using Away.App.Domain.Setting;
using Away.App.Domain.Xray.Models;

namespace Away.App.Domain.Xray.Impl;

[DI]
public sealed class XraySettingService(IAppSettingRepository appSetting) : IXraySettingService
{
    private const string KEY = "SpeedTestSettings";

    public SpeedTestSettings Get()
    {
        var settings = appSetting.Get<SpeedTestSettings>(KEY);
        if (settings == null)
        {
            settings = new();
            Set(settings);
        }

        return settings;
    }

    public bool Set(SpeedTestSettings model)
    {
        return appSetting.Set(KEY, model);
    }

}
