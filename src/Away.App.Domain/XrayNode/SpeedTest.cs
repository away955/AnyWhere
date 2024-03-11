using Away.Domain.Xray.Impl;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace Away.Domain.XrayNode;

public sealed class SpeedTest : BaseXrayService, IDisposable
{
    private const string Host = "127.0.0.1";
    /// <summary>
    /// 测试地址
    /// </summary>
    private const string TestUrl = "http://hel1-speed.hetzner.com/100MB.bin";
    /// <summary>
    /// 测试时间
    /// </summary>
    private const int TestSeconds = 10;

    public event Action<SpeedTestResult>? OnResult;
    private readonly CancellationTokenSource _cts;
    private readonly XrayNodeEntity entity;
    private readonly int port;
    public SpeedTest(XrayNodeEntity entity, int port, string configFileName, int timeout) : base(configFileName)
    {
        this.entity = entity;
        this.port = port;
        this._cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
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

    protected override void OnMessage(string msg)
    {
        if (string.IsNullOrWhiteSpace(msg))
        {
            return;
        }
        base.OnMessage(msg);
        // v2ray 启动成功
        var regStartUp = Regex.Match(msg, "V2Ray.*.started");
        if (regStartUp.Success)
        {
            Task.Run(async () =>
            {
                var speedRes = await TestDownload();
                XrayClose();
                OnResult?.Invoke(speedRes);              
            });
        }
        // v2ray 启动失败
        var regStartFailed = Regex.Match(msg, "Failed to start");
        if (regStartFailed.Success)
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

    private async Task<SpeedTestResult> TestDownload()
    {
        try
        {
            using var httpclient = new HttpClient(new HttpClientHandler
            {
                Proxy = new WebProxy(Host, port),
            })
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            using var resp = await httpclient.GetAsync(TestUrl, HttpCompletionOption.ResponseHeadersRead, _cts.Token);
            resp.EnsureSuccessStatusCode();
            using var stream = await resp.Content.ReadAsStreamAsync();
            Stopwatch stopwatch = Stopwatch.StartNew();
            var count = 0;
            var len = 0;
            byte[] bytes = new byte[1024];
            while ((len = await stream.ReadAsync(bytes)) > 0)
            {
                count += len;
                if (stopwatch.ElapsedMilliseconds / 1000d >= TestSeconds)
                {
                    break;
                }
            }
            stopwatch.Stop();
            var sec = stopwatch.ElapsedMilliseconds / 1000d;

            // 下载速度 b/s
            var speed = count / sec;
            var remark = speed switch
            {
                var i when 0 < i && i < 1024 => $"{Math.Round(speed, 2)} b/s",
                var i when 1024 < i && i < 1024 * 1024 => $"{Math.Round(speed / 1024, 2)} kb/s",
                var i when 1024 * 1024 < i => $"{Math.Round(speed / 1024 / 1024, 2)} m/s",
                _ => string.Empty
            };
            return new SpeedTestResult
            {
                Entity = entity,
                IsSuccess = true,
                Speed = speed,
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