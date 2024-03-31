namespace Away.App.Domain.Youtube.Models;

public class FileModel
{
    public string RootPath { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;

    public string RootFolderPath => Path.Combine(RootPath, RelativePath);
    public string FileRelativePath => Path.Combine(RelativePath, $"{FileName}{FileExtension}");
    public string FileRootPath => Path.Combine(RootFolderPath, $"{FileName}{FileExtension}");

    public void DeleteFolder()
    {
        Directory.Delete(RootFolderPath, true);
    }
}

public sealed class DownloadBuilder : FileModel
{
    public DownloadBuilder()
    {
        var time = DateTime.Now;
        FileName = Convert.ToString(time.ToFileTime());
    }

    public void AddRelativePath(string path)
    {
        this.RelativePath = Path.Combine(RelativePath, path);
    }

    public FileModel Build()
    {
        var filepath = Path.Combine(RootPath, RelativePath);
        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }
        return this;
    }
}