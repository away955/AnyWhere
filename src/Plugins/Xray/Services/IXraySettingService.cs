namespace Xray.Services;

public interface IXraySettingService
{
    SpeedTestSettings Get();
    bool Set(SpeedTestSettings model);
}
