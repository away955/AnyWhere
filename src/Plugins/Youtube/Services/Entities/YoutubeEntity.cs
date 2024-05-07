namespace Youtube.Services.Entities;

[SugarTable("video_youtube")]
public sealed class YoutubeEntity
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    /// <summary>
    /// 源地址
    /// </summary>
    public string Source { get; set; } = string.Empty;
    /// <summary>
    /// 视频地址
    /// </summary>
    public string VideoPath { get; set; } = string.Empty;
    public string VideoArea { get; set; } = string.Empty;
    /// <summary>
    /// 图片地址
    /// </summary>
    public string ImagePath { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    /// <summary>
    /// 上传时间
    /// </summary>
    public DateTime Uploaded { get; set; }
    public YoutubeVideoState State { get; set; }
}

public enum YoutubeVideoState
{
    Error = -1,
    Waiting = 0,
    Downloading = 1,
    Success = 2
}
