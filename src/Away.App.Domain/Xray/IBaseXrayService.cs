namespace Away.Domain.Xray;

public interface IBaseXrayService
{
    bool IsEnable { get; }
    XrayConfig Config { get; }
    XrayConfig? GetConfig();
    void SetConfig(XrayConfig xrayConfig);
    void SetNode(XrayNodeEntity node);
    void SaveConfig();
    void XrayRestart();
    bool XrayStart();
    bool XrayClose();
}
