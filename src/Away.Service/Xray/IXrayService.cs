namespace Away.Service.Xray;

public interface IXrayService
{
    XrayConfig? GetConfig();
    void SetConfig(XrayConfig xrayConfig);
    void Run();
}
