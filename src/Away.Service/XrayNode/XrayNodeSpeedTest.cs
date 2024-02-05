using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Channels;
using Away.Service.XrayNode.Impl;

namespace Away.Service.XrayNode;

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
    public event Action<SpeedTestResultEventArgs>? OnCompeleted;
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
    private int _total;
    /// <summary>
    /// 已检测的节点数
    /// </summary>
    private int _count;

    public void Cancel() => _cts.Cancel();

    public void Listen()
    {
        Task.Run(() =>
        {
            while (!_cts.IsCancellationRequested)
            {
                _semaphore.WaitOne();
                if (_count == _total)
                {
                    Log.Information("节点测试完成");
                    break;
                }
                _ = RunOne();
            }
        });
    }

    public async Task RunOne()
    {
        var entity = await _queue.Reader.ReadAsync(_cts.Token);
        if (entity == null)
        {
            return;
        }

        // 获取未使用的端口、并修改端口状态
        var port = Ports.Where(o => o.Value == false).FirstOrDefault().Key;
        if (port == 0)
        {
            await _queue.Writer.WriteAsync(entity, _cts.Token);
            return;
        }

        Ports.TryUpdate(port, true, false);
        var service = new BaseSpeedTest(port, $"speed_test_{port}.json");
        var result = await service.TestSpeed(entity);
        OnCompeleted?.Invoke(new SpeedTestResultEventArgs
        {
            Data = result,
            XrayNode = entity
        });
        Ports.TryUpdate(port, false, true);
        _count++;
        _semaphore.Release();
    }
}

public sealed class SpeedTestResultEventArgs : EventArgs
{
    public required SpeedTestResult Data { get; set; }
    public required XrayNodeEntity XrayNode { get; set; }
}