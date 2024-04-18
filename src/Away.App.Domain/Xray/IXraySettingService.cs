using Away.App.Domain.Xray.Models;

namespace Away.App.Domain.Xray;

public interface IXraySettingService
{
    SpeedTestSettings Get();
    bool Set(SpeedTestSettings model);
}
