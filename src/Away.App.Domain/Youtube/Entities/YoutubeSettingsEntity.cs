using SqlSugar;

namespace Away.App.Domain.Youtube.Entities;

[SugarTable("video_youtube_settings")]
public sealed class YoutubeSettingsEntity
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
}
