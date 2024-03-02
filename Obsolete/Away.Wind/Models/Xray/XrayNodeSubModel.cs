namespace Away.Wind.Models;

public class XrayNodeSubModel : BindableBase
{
    private int _id;
    private string _url = string.Empty;
    private bool _isDisable;
    private string _remark = string.Empty;

    public int Id { get => _id; set => SetProperty(ref _id, value); }

    public string Url { get => _url; set => SetProperty(ref _url, value); }

    public bool IsDisable { get => _isDisable; set => SetProperty(ref _isDisable, value); }

    public string Remark { get => _remark; set => SetProperty(ref _remark, value); }
}
