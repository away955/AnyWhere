namespace Away.App.Services.Models;

/// <summary>
/// 更新商店资源
/// </summary>
public sealed class StoreResource
{
    /// <summary>
    /// 授权码
    /// </summary>
    public required string Authorization { get; set; }

    /// <summary>
    /// App信息
    /// </summary>
    public required AppResource App { get; set; }

    /// <summary>
    /// 更新程序信息
    /// </summary>
    public required AppResource Updated { get; set; }

    /// <summary>
    /// 插件资源集合
    /// </summary>
    public required List<PluginStoreEntity> Plugins { get; set; }
}

/// <summary>
/// App资源
/// </summary>
public sealed class AppResource : VersionBase
{
    public required string ContentID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Updated { get; set; }
}

/// <summary>
/// 版本
/// </summary>
public abstract class VersionBase
{
    public required string Version { get; set; }
    private int _versionNumber;
    public int VersionNumber
    {
        get
        {
            if (_versionNumber == 0)
            {
                _versionNumber = ParseVersion(Version);
            }
            return _versionNumber;
        }
    }

    public bool HasNewVersion(string currentVersion)
    {
        return VersionNumber > ParseVersion(currentVersion);
    }

    private static int ParseVersion(string ver)
    {
        if (string.IsNullOrWhiteSpace(ver))
        {
            return 0;
        }
        var arr = ver.Split('.', StringSplitOptions.TrimEntries);
        if (arr.Length != 3)
        {
            return 0;
        }
        return Convert.ToInt32(arr[0]) * 100000
            + Convert.ToInt32(arr[1]) * 1000
            + Convert.ToInt32(arr[2]) * 10;
    }
}