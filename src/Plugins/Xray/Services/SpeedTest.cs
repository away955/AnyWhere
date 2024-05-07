using Xray.Services.Impl;

namespace Xray.Services;

public sealed class SpeedTest : XrayServiceBase, IDisposable
{
    private const string Host = "127.0.0.1";

    private readonly SpeedTestSettings _settings;
    private readonly CancellationTokenSource _cts;
    private readonly XrayNodeEntity entity;
    private readonly int port;

    public event Action<SpeedTestResult>? OnResult;

    public SpeedTest(XrayNodeEntity entity, string configFileName, int port, SpeedTestSettings settings) : base(configFileName)
    {
        this.entity = entity;
        _settings = settings;
        this.port = port;
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(settings.TestTimeout));
        _cts.Token.Register(() =>
        {
            if (!IsEnable)
            {
                return;
            }
            Log.Warning("v2ray 测速超时取消");
            XrayClose();
            OnResult?.Invoke(new SpeedTestResult { Entity = entity, Error = "测试超时" });
        });
    }

    public void Dispose()
    {
        XrayClose();
        _cts.Dispose();
    }

    protected override void OnMessage(string msg, V2rayState state)
    {
        if (string.IsNullOrWhiteSpace(msg))
        {
            return;
        }
        // v2ray 启动成功
        if (state == V2rayState.Started)
        {
            Task.Run(async () =>
            {
                //var res = await TestDownload();
                var res = await TestTelnet();
                XrayClose();
                OnResult?.Invoke(res);
            });
        }
        // v2ray 启动失败
        if (state == V2rayState.FailedStart)
        {
            XrayClose();
            OnResult?.Invoke(new SpeedTestResult { Entity = entity, Error = "启动失败" });
        }
    }

    public void TestSpeed()
    {
        var flag = SetTestConfig();
        if (!flag)
        {
            OnResult?.Invoke(new SpeedTestResult { Entity = entity, Error = "设置代理失败" });
            return;
        }
        XrayStart();
    }

    private bool SetTestConfig()
    {
        Config.inbounds.Clear();
        Config.routing = null;
        Config.SetInbound(new XrayInbound()
        {
            listen = Host,
            port = port,
            tag = "node_test",
            protocol = "http",
        });
        var flag = Config.SetOutbound(entity);
        if (!flag)
        {
            Log.Warning($"设置代理失败:{entity.Url}");
            return false;
        }
        SaveConfig();
        return true;
    }

    //private async Task<SpeedTestResult> TestDownload()
    //{
    //    try
    //    {
    //        using var httpclient = new HttpClient(new HttpClientHandler
    //        {
    //            Proxy = new WebProxy(Host, port),
    //        })
    //        {
    //            Timeout = TimeSpan.FromSeconds(_settings.TestTimeout)
    //        };
    //        httpclient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0");

    //        using var resp = await httpclient.GetAsync(_settings.TestUrl, HttpCompletionOption.ResponseHeadersRead, _cts.Token);
    //        resp.EnsureSuccessStatusCode();
    //        using var stream = await resp.Content.ReadAsStreamAsync();
    //        Stopwatch stopwatch = Stopwatch.StartNew();
    //        var count = 0;
    //        var len = 0;
    //        byte[] bytes = new byte[1024];
    //        while ((len = await stream.ReadAsync(bytes)) > 0)
    //        {
    //            count += len;
    //            if (stopwatch.ElapsedMilliseconds / 1000d >= 10)
    //            {
    //                break;
    //            }
    //        }
    //        stopwatch.Stop();
    //        var sec = stopwatch.ElapsedMilliseconds / 1000d;

    //        // 下载速度 b/s
    //        var speed = count / sec;
    //        var remark = speed switch
    //        {
    //            var i when 0 < i && i < 1024 => $"{Math.Round(speed, 2)} b/s",
    //            var i when 1024 < i && i < 1024 * 1024 => $"{Math.Round(speed / 1024, 2)} kb/s",
    //            var i when 1024 * 1024 < i => $"{Math.Round(speed / 1024 / 1024, 2)} m/s",
    //            _ => string.Empty
    //        };
    //        return new SpeedTestResult
    //        {
    //            Entity = entity,
    //            IsSuccess = true,
    //            Speed = speed,
    //            Remark = remark,
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Information($"测试节点速度失败:{ex.Message}");
    //        return new SpeedTestResult { Entity = entity, Error = ex.Message };
    //    }
    //}

    private async Task<SpeedTestResult> TestTelnet()
    {
        try
        {
            using var httpclient = new HttpClient(new HttpClientHandler
            {
                Proxy = new WebProxy(Host, port),
            })
            {
                Timeout = TimeSpan.FromSeconds(_settings.TestTimeout - 1)
            };
            httpclient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0");

            Stopwatch stopwatch = Stopwatch.StartNew();
            using var resp = await httpclient.GetAsync(_settings.TestUrl, HttpCompletionOption.ResponseHeadersRead, _cts.Token);
            resp.EnsureSuccessStatusCode();
            stopwatch.Stop();

            var remark = stopwatch.ElapsedMilliseconds switch
            {
                var i when 0 < i && i < 1000 => $"{i} ms",
                var i when 100 < i => $"{Math.Round(i / 1000d, 2)} s",
                _ => string.Empty
            };
            return new SpeedTestResult
            {
                Entity = entity,
                IsSuccess = true,
                Speed = stopwatch.ElapsedMilliseconds,
                Remark = remark,
            };
        }
        catch (Exception ex)
        {
            Log.Information($"测试节点速度失败:{ex.Message}");
            return new SpeedTestResult { Entity = entity, Error = ex.Message };
        }
    }
}