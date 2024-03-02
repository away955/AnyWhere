namespace Away.App.Core.Repository;

public interface IFileContext
{
    public FileContextOptions Options { get; }
    List<T> AsQueryable<T>() where T : class;
    void Save<T>(IEnumerable<T> data) where T : class;
}

public sealed class FileContextOptions
{
    public string FolderPathBase { get; set; } = "Data";
    public string Extension { get; set; } = "json";
}

public sealed class FileContext(FileContextOptions options) : IFileContext
{
    public FileContextOptions Options => options;

    public List<T> AsQueryable<T>() where T : class
    {
        var filepath = GetFilePath<T>();
        if (!File.Exists(filepath))
        {
            File.Create(filepath).Dispose();
            return [];
        }

        var bytes = File.ReadAllBytes(filepath);
        if (bytes?.Length == 0)
        {
            return [];
        }
        return JsonUtils.Deserialize<List<T>>(bytes) ?? [];
    }

    public void Save<T>(IEnumerable<T> data) where T : class
    {
        var filepath = GetFilePath<T>();
        var bytes = JsonSerializer.SerializeToUtf8Bytes(data);
        File.WriteAllBytes(filepath, bytes);
    }

    private string GetFilePath<T>()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Options.FolderPathBase);
        var filename = $"{typeof(T).Name.ToLower()}.{Options.Extension}";
        return Path.Combine(path, filename);
    }
}
