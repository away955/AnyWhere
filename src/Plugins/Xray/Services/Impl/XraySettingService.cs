namespace Xray.Services.Impl;
 
public sealed class XraySettingService(IAppSettingService appSetting) : IXraySettingService
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
