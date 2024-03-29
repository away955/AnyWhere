namespace Away.App.Domain.Youtube.Entities;

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
    public string VideoUrl { get; set; } = string.Empty;
    /// <summary>
    /// 图片地址
    /// </summary>
    public string Thumbnail { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTimeOffset UploadDate { get; set; }

    public YoutubeVideoState State { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

public enum YoutubeVideoState
{
    Error = -1,
    Waiting = 0,
    Success = 1
}
