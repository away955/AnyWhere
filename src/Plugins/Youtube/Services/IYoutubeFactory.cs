using System.Net;

namespace Youtube.Services;

public interface IYoutubeFactory
{
    event Action<int, double>? DownloadProgress;
    IWebProxy? Proxy { get; set; }

    bool Cancel(int id);
    bool AddTask(int id);
}
