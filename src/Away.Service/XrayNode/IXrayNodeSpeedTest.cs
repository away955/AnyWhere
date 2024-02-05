using Away.Service.XrayNode.Impl;

namespace Away.Service.XrayNode;

public interface IXrayNodeSpeedTest
{
    Task<SpeedTestResult> TestSpeed(XrayNodeEntity entity);
}
