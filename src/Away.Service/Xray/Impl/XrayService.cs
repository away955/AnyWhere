using Away.Service.Xray.Model;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Away.Service.Xray.Impl;

[ServiceInject]
public class XrayService : IXrayService
{
    private readonly ILogger<XrayService> _logger;
    private readonly string _xrayConfigPath;
    private readonly string _xrayPath;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private XrayConfig? _xrayConfig;

    public XrayService(ILogger<XrayService> logger)
    {
        _logger = logger;
        _xrayConfig = new XrayConfig();
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        };

        var baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Xray");
        _xrayConfigPath = Path.Combine(baseDirectory, "config.json");
        _xrayPath = Path.Combine(baseDirectory, "xray");

        // 初始化配置文件
        if (!File.Exists(_xrayConfigPath))
        {
            SetConfig(_xrayConfig);
        }
        GetConfig();
        //File.WriteAllText(Path.Combine(baseDirectory, "config2.json"), Serialize(_xrayConfig));
    }

    public void SetConfig(XrayConfig xrayConfig)
    {
        File.WriteAllText(_xrayConfigPath, Serialize(xrayConfig));
    }

    public XrayConfig? GetConfig()
    {
        var json = File.ReadAllText(_xrayConfigPath);
        _xrayConfig = Deserialize<XrayConfig>(json);
        return _xrayConfig;
    }

    public void Run()
    {
        Process process = new();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = _xrayPath,
        };

        process.OutputDataReceived += (sender, args) =>
        {
            _logger.LogInformation(args.Data);
        };
        process.Start();
    }


    private string Serialize<T>(T t)
    {
        return JsonSerializer.Serialize(t, _jsonSerializerOptions);
    }
    private T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
    }
}
