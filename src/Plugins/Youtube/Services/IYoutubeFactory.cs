namespace Youtube.Services;

public interface IYoutubeFactory
{
    event Action<int, double>? DownloadProgress;

    bool Cancel(int id);
    bool AddTask(int id);
}
