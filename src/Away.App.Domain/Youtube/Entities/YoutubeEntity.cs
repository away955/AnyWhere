namespace Away.App.Domain.Youtube.Entities;

[SugarTable("video_youtube")]
public sealed class YoutubeEntity
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int State { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
