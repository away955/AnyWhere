namespace Away.Domain.XrayNode.Model;

public sealed class SpeedTestResult
{
    public required XrayNodeEntity Entity { get; set; }
    public bool IsSuccess { get; set; }
    public string Speed { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}