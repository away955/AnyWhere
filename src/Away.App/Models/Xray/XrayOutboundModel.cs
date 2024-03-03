namespace Away.App.Models;

public sealed class XrayOutboundModel
{
    public string protocol { get; set; } = string.Empty;
    public string tag { get; set; } = string.Empty;
    public string SettingStr { get; set; } = string.Empty;
    public bool IsVisibleSettingStr => !string.IsNullOrWhiteSpace(SettingStr);
}
