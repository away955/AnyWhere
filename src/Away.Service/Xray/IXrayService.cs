namespace Away.Service.Xray;

public interface IXrayService
{
    bool IsOpened { get; }
    XrayConfig Config { get; }
    XrayConfig? GetConfig();
    void SetConfig(XrayConfig xrayConfig);
    void SaveConfig();

    void XrayStart();
    void XrayClose();
}
