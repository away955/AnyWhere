using Away.Service.Xray.Impl;
using System.Diagnostics;
using System.Net;

namespace Away.Service.XrayNode.Impl;

[ServiceInject(ServiceLifetime.Singleton)]
public class XrayNodeSpeedTest : BaseXrayService, IXrayNodeSpeedTest
{
    private const string ExeFileName = "v2ray";
    private const string Host = "127.0.0.1";
    private const int Port = 2024;
    /// <summary>
    /// 测试地址
    /// </summary>
    private const string TestUrl = "http://hel1-speed.hetzner.com/100MB.bin";
    /// <summary>
    /// 测试时间
    /// </summary>
    private const int TestSeconds = 10;

    public XrayNodeSpeedTest(ILogger<XrayService> logger) : base(logger, ExeFileName, "speed_test.json")
    {
    }

    public async Task<SpeedTestResult> TestSpeed(XrayNodeEntity entity)
    {
        SetTestConfig(entity);
        XrayStart();
        var speedRes = await TestDownload();
        XrayClose();

        return speedRes;
    }

    private void SetTestConfig(XrayNodeEntity entity)
    {
        Config.inbounds.Clear();
        Config.SetInbound(new XrayInbound()
        {
            listen = Host,
            port = Port,
            tag = "node_test",
            protocol = "http",
        });
        Config.SetOutbound(entity);
        SaveConfig();
    }

    private async Task<SpeedTestResult> TestDownload()
    {
        try
        {
            using var httpclient = new HttpClient(new HttpClientHandler
            {
                Proxy = new WebProxy(Host, Port),
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
            _logger.LogInformation("测试节点速度失败:{0}", ex.Message);
            return new SpeedTestResult { Error = "不可用" };
        }
    }
}

public sealed class SpeedTestResult
{
    public bool IsSuccess { get; set; }
    public string Speed { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
