using System.Windows.Input;

namespace RouterScanner.ViewModels;

public sealed class RouterViewModel : ViewModelBase
{
    private readonly IRouterScanner _routerScanner;

    /// <summary>
    /// 启动
    /// </summary>
    public ICommand StartCommand { get; }
    /// <summary>
    /// 暂停
    /// </summary>
    public ICommand AboutCommand { get; }
    /// <summary>
    /// 停止
    /// </summary>
    public ICommand StopCommand { get; }
    /// <summary>
    /// 漏洞利用
    /// </summary>
    public ICommand ExpCommand { get; }


    /// <summary>
    /// 路由列表
    /// </summary>
    [Reactive]
    public ObservableCollection<RouterModel> Items { get; set; } = [];

    /// <summary>
    /// 是否运行中
    /// </summary>
    [Reactive]
    public bool IsRunning { get; set; }

    #region 参数

    /// <summary>
    /// 扫描IP<br/>
    /// <code>
    /// 192.168.1.1
    /// 192.168.1.1-192.168.1.255
    /// </code>
    /// </summary>
    public string IPs { get; set; } = "192.168.2.1";
    /// <summary>
    /// 扫描的端口号<br/>
    /// <code>
    /// 80
    /// 80-90
    /// </code>
    /// </summary>
    public string Ports { get; set; } = "80";
    /// <summary>
    /// 指纹扫描线程数
    /// </summary>
    public int FingerPrintThreads { get; set; } = 10;
    /// <summary>
    /// 指纹扫描超时时间/毫秒
    /// </summary>
    public int FingerPrintTimeout { get; set; } = 1000 * 3;
    /// <summary>
    /// 漏洞扫描线程数
    /// </summary>
    public int VulThreads { get; set; } = 10;
    /// <summary>
    /// 漏洞扫描超时时间/毫秒
    /// </summary>
    public int VulTimeout { get; set; } = 1000 * 3;

    #endregion

    public RouterViewModel(IRouterScanner routerScanner)
    {
        _routerScanner = routerScanner;
        routerScanner.OnFingerPrintCompleted += OnFingerPrintCompleted;
        routerScanner.OnVulCompleted += OnVulCompleted;
        routerScanner.OnCompleted += OnCompleted;

        StartCommand = ReactiveCommand.Create(OnStart);
        AboutCommand = ReactiveCommand.Create(OnAbout);
        StopCommand = ReactiveCommand.Create(OnStop);
        ExpCommand = ReactiveCommand.Create<IRouterVulHub>(OnExp);
    }

    private void OnCompleted()
    {
        IsRunning = false;
    }

    private void OnVulCompleted(VulResult obj)
    {
        var model = Items.FirstOrDefault(o => o.Url == obj.Vul.Url);
        if (model == null)
        {
            return;
        }
        model.Vuls.Add(obj.Vul);
    }

    private void OnFingerPrintCompleted(FingerPrintResult fpr)
    {
        var model = new RouterModel
        {
            Url = fpr.Url,
        };
        if (fpr.Result != null)
        {
            model.Production = fpr.Result.Production;
            model.Version = fpr.Result.Version;
            model.Firmware = fpr.Result.Firmware;
        }
        Items.Add(model);

    }

    private void OnStart()
    {
        IsRunning = true;
        _routerScanner.IPs = IPs;
        _routerScanner.Ports = Ports;
        _routerScanner.FingerPrintThreads = FingerPrintThreads;
        _routerScanner.FingerPrintTimeout = FingerPrintTimeout;
        _routerScanner.VulThreads = VulThreads;
        _routerScanner.VulTimeout = VulTimeout;
        _routerScanner.Run();
    }

    private void OnStop()
    {
        _routerScanner.Cancel();
        IsRunning = false;
    }

    private void OnAbout()
    {
        Items.Add(new RouterModel { Url = "a" });
        
        IsRunning = false;
    }

    private void OnExp(IRouterVulHub vul)
    {
        ViewRouter.Go("router-exp", vul);
    }
}
