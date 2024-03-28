namespace Away.App.Models;

public sealed class YoutubeModel
{
    [Reactive]
    public int Id { get; set; }
    [Reactive]
    public string Title { get; set; } = string.Empty;
    [Reactive]
    public string Description { get; set; } = string.Empty;
    [Reactive]
    public string Url { get; set; } = string.Empty;
    [Reactive]
    public int State { get; set; }
    [Reactive]
    public DateTime Created { get; set; }
    [Reactive]
    public DateTime Updated { get; set; }
}
