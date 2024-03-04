using Away.Domain.Xray.Impl;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace Away.Domain.XrayNode;

public sealed class SpeedTest(int port, string configFileName) : BaseXrayService(configFileName)
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
    private int _port = port;

    public event Action<SpeedTestResult>? OnResult;

    protected override async void OnMessage(object sender, DataReceivedEventArgs e)
    {       
        var msg = e.Data;
        var reg = Regex.Match(msg,"V2Ray.*.started");
        if(reg.Success)
        {        
            var speedRes = await TestDownload();
            OnResult?.Invoke(speedRes);
            XrayClose();
        }     
        base.OnMessage(sender,e);
    }

    public void TestSpeed(XrayNodeEntity entity)
    {
        var flag = SetTestConfig(entity);
        if (!flag)
        {
            OnResult?.Invoke(new SpeedTestResult { Error = "设置代理失败" });
            return;
        }     
        XrayStart();
    }

    private bool SetTestConfig(XrayNodeEntity entity)
    {
        Config.inbounds.Clear();
        Config.SetInbound(new XrayInbound()
        {
            listen = Host,
            port = _port,
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
                Proxy = new WebProxy(Host, _port),
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
                IsSuccess = true,
                Speed = speed,
            };
        }
        catch (Exception ex)
        {
            Log.Information($"测试节点速度失败:{ex.Message}");
            return new SpeedTestResult { Error = "不可用" };
        }
    }
}