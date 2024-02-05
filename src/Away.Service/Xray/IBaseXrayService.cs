namespace Away.Service.Xray;

public interface IBaseXrayService
{
    bool IsOpened { get; }
    XrayConfig Config { get; }
    XrayConfig? GetConfig();
    void SetConfig(XrayConfig xrayConfig);
    void SaveConfig();
    void XrayRestart();
    void XrayStart();
    void XrayClose();
}
