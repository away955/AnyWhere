namespace Away.Service.XrayNode.Model;

public sealed class SpeedTestResult
{
    public bool IsSuccess { get; set; }
    public string Speed { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}