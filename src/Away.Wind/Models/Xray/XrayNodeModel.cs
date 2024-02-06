namespace Away.Wind.Models;

public class XrayNodeModel : BindableBase
{
    private int _id;
    private string _type = string.Empty;
    private string _alias = string.Empty;
    private string _host = string.Empty;
    private int _port;
    private string _url = string.Empty;
    private XrayNodeStatus _status;
    private string _remark = string.Empty;
    private bool _isChecked;

    public int Id { get => _id; set => SetProperty(ref _id, value); }

    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get => _type; set => SetProperty(ref _type, value); }

    /// <summary>
    /// 别名
    /// </summary>
    public string Alias { get => _alias; set => SetProperty(ref _alias, value); }

    /// <summary>
    /// 地址
    /// </summary>
    public string Host { get => _host; set => SetProperty(ref _host, value); }

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get => _port; set => SetProperty(ref _port, value); }

    /// <summary>
    /// 原Url
    /// </summary>
    public string Url { get => _url; set => SetProperty(ref _url, value); }

    /// <summary>
    /// 状态
    /// </summary>
    public XrayNodeStatus Status { get => _status; set => SetProperty(ref _status, value); }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get => _remark; set => SetProperty(ref _remark, value); }

    /// <summary>
    /// 是否使用
    /// </summary>
    public bool IsChecked { get => _isChecked; set => SetProperty(ref _isChecked, value); }
}
