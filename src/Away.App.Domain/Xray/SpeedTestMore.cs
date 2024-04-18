using System.Collections.Concurrent;
using System.Runtime;
using Away.App.Domain.Xray.Entities;
using Away.App.Domain.Xray.Models;

namespace Away.App.Domain.Xray;

public sealed class SpeedTestMore
{
    private readonly CancellationTokenSource _cts = new();
    private readonly Semaphore _semaphore;
    private readonly List<XrayNodeEntity> _nodes;
    private readonly SpeedTestSettings _settings;


    public SpeedTestMore(List<XrayNodeEntity> entities, SpeedTestSettings settings)
    {
        _settings = settings;
        _semaphore = new(settings.Concurrency, settings.Concurrency);
        foreach (var port in Enumerable.Range(settings.StartPort, settings.Concurrency))
        {
            Ports.TryAdd(port, false);
        }

        _nodes = entities;
        _total = entities.Count;
        _cts.Token.Register(() =>
        {
            OnCancel?.Invoke();
        });
    }

    /// <summary>
    /// 检测完成
    /// </summary>
    public event Action<SpeedTestResult>? OnTested;

    /// <summary>
    /// 全部完成
    /// </summary>
    public event Action? OnCompeleted;

    /// <summary>
    /// 测试进度
    /// </summary>
    public event Action<int>? OnProgress;

    /// <summary>
    /// 取消
    /// </summary>
    public event Action? OnCancel;

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
        _cts.Cancel();
    }

    public void Listen()
    {
        Task.Run(() =>
        {
            foreach (var entity in _nodes)
            {
                if (_cts.IsCancellationRequested)
                {
                    break;
                }
                _semaphore.WaitOne();
                Task.Delay(200).Wait();
                var port = Ports.Where(o => o.Value == false).FirstOrDefault().Key;
                if (port == 0)
                {
                    continue;
                }
                Ports.TryUpdate(port, true, false);
                RunOne(entity, port);
            }
            Log.Information("测试结束，退出循环");
        });
    }

    private void RunOne(XrayNodeEntity entity, int port)
    {
        try
        {
            var service = new SpeedTest(entity, $"speed_test_{port}.json", port, _settings);
            service.OnResult += Tested;
            service.TestSpeed();
            void Tested(SpeedTestResult result)
            {
                Log.Information($"进度：{_count}/{_total} 结果：{result.Remark} {result.Error}");
                if (_cts.IsCancellationRequested)
                {
                    return;
                }
                OnTested?.Invoke(result);
                Release(port);
                service.OnResult -= Tested;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            Release(port);
        }
    }

    private void Release(int port)
    {
        Ports.TryUpdate(port, false, true);
        _count++;
        var progressValue = (int)(_count * 1d / _total * 100);
        OnProgress?.Invoke(progressValue);
        if (_count == _total)
        {
            OnCompeleted?.Invoke();
        }
        _semaphore.Release();
    }
}