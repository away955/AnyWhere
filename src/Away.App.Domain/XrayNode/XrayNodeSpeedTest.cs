using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Away.Domain.XrayNode;

public sealed class XrayNodeSpeedTest
{
    private readonly CancellationTokenSource _cts = new();
    private readonly Channel<XrayNodeEntity> _queue;
    private readonly Semaphore _semaphore;
    public XrayNodeSpeedTest(List<XrayNodeEntity> entities, int concurrency, int startPort)
    {
        Concurrency = concurrency;
        StartPort = startPort;
        _semaphore = new(concurrency, concurrency);
        _queue = Channel.CreateBounded<XrayNodeEntity>(Concurrency);

        foreach (var port in Enumerable.Range(startPort, Concurrency))
        {
            Ports.TryAdd(port, false);
        }

        _total = entities.Count;
        foreach (var entity in entities)
        {
            Task.Run(async () =>
            {
                await _queue.Writer.WriteAsync(entity, _cts.Token);
                OnTesting?.Invoke(entity);
            });
        }
    }

    /// <summary>
    /// 检测中
    /// </summary>
    public event Action<XrayNodeEntity>? OnTesting;

    /// <summary>
    /// 检测完成
    /// </summary>
    public event Action<SpeedTestResult>? OnTested;

    /// <summary>
    /// 全部完成
    /// </summary>
    public event Action? OnCompeleted;

    /// <summary>
    /// 取消
    /// </summary>
    public event Action? OnCancel;

    /// <summary>
    /// 并发数
    /// </summary>
    public int Concurrency { get; private set; }
    /// <summary>
    /// 开始端口
    /// </summary>
    public int StartPort { get; private set; }


    /// <summary>
    /// 可用端口，true：使用中，false：未使用
    /// </summary>
    private readonly ConcurrentDictionary<int, bool> Ports = new();

    /// <summary>
    /// 总节点数
    /// </summary>
    private readonly int _total;
    /// <summary>
    /// 已检测的节点数
    /// </summary>
    private int _count;

    public void Cancel()
    {
        OnCancel?.Invoke();
        _cts.Cancel();
    }

    public void Listen()
    {
        Task.Run(async () =>
        {
            while (!_cts.IsCancellationRequested)
            {
                _semaphore.WaitOne();
                if (_count == _total)
                {
                    Log.Information("节点测试完成");
                    if (OnCompeleted != null)
                    {
                        OnCompeleted.Invoke();
                        await Task.Delay(500);
                    }
                    break;
                }
                _ = RunOne();
            }
        });
    }

    public async Task RunOne()
    {
        var port = Ports.Where(o => o.Value == false).FirstOrDefault().Key;
        Ports.TryUpdate(port, true, false);

        try
        {
            var entity = await _queue.Reader.ReadAsync(_cts.Token);
            if (entity == null)
            {
                return;
            }

            if (port == 0)
            {
                await _queue.Writer.WriteAsync(entity, _cts.Token);
                return;
            }

            var service = new SpeedTest(entity, port, $"speed_test_{port}.json", 40);
            Action<SpeedTestResult> Tested = (result) =>
            {
                OnTested?.Invoke(result);
                Ports.TryUpdate(port, false, true);
                _count++;
                _semaphore.Release();
            };

            service.OnResult += (result) => Tested(result);
            service.TestSpeed();
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            Ports.TryUpdate(port, false, true);
            _count++;
            _semaphore.Release();
        }
    }
}