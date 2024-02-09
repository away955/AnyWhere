namespace Away.Wind.Models;

public class XrayInboundModel : BindableBase
{
    private int _port;
    private string _protocol = string.Empty;
    private string _tag = string.Empty;

    public int port
    {
        get => _port;
        set => SetProperty(ref _port, value);
    }
    public string protocol
    {
        get => _protocol;
        set => SetProperty(ref _protocol, value);
    }
    /// <summary>
    /// 此入站连接的标识 唯一
    /// </summary>
    public string tag
    {
        get => _tag;
        set => SetProperty(ref _tag, value);
    }
}
