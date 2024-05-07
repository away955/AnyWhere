namespace Youtube.Models;

public sealed class YoutubeVideoModel : ViewModelBase
{
    [Reactive]
    public string Url { get; set; } = string.Empty;
    /// <summary>
    /// 视频像素
    /// </summary>
    [Reactive]
    public string Area { get; set; } = string.Empty;
    /// <summary>
    /// 文件大小 MB
    /// </summary>
    [Reactive]
    public double FileSize { get; set; }
    /// <summary>
    /// 文件格式
    /// </summary>
    [Reactive]
    public string FileExtentsion { get; set; } = string.Empty;
    [Reactive]
    public Bitmap? Bitmap { get; set; }
}
