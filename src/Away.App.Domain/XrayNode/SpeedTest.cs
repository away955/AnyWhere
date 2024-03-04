using Away.Domain.Xray.Impl;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace Away.Domain.XrayNode;

public sealed class SpeedTest(XrayNodeEntity entity, int port, string configFileName, int timeout) : BaseXrayService(configFileName)
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

    protected override void OnMessage(string msg)
    {
        if (string.IsNullOrWhiteSpace(msg))
        {
            return;
        }
        base.OnMessage(msg);
        // v2ray 启动成功
        var reg = Regex.Match(msg, "V2Ray.*.started");
        if (reg.Success)
        {
            Task.Run(async () =>
            {
                var speedRes = await TestDownload();
                OnResult?.Invoke(speedRes);
                XrayClose();
            });
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
        Task.Run(() =>
        {
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (!IsEnable)
                {
                    break;
                }
                if (stopwatch.ElapsedMilliseconds / 1000d >= timeout)
                {
                    break;
                }
            }
            stopwatch.Stop();
            if (IsEnable)
            {
                OnResult?.Invoke(new SpeedTestResult { Entity = entity, Error = "测试超时" });
                XrayClose();
            }
        });

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

            using var stream = await httpclient.GetStreamAsync(TestUrl);

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
            var ps = count / sec;
            var speed = ps switch
            {
                var i when 0 < i && i < 1024 => $"{Math.Round(ps, 2)} b/s",
                var i when 1024 < i && i < 1024 * 1024 => $"{Math.Round(ps / 1024, 2)} kb/s",
                var i when 1024 * 1024 < i => $"{Math.Round(ps / 1024 / 1024, 2)} m/s",
                _ => string.Empty
            };
            return new SpeedTestResult
            {
                Entity = entity,
                IsSuccess = true,
                Speed = speed,
            };
        }
        catch (Exception ex)
        {
            Log.Information($"测试节点速度失败:{ex.Message}");
            return new SpeedTestResult { Entity = entity, Error = ex.Message };
        }
    }
}