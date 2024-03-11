namespace Away.App.Update;

public sealed class AppInfo
{
    public const string Version = "v1.0.0";
    public const string Title = "哪都通更新程序";

    public const string Host = "https://proxy.v2gh.com/https://raw.githubusercontent.com/away955/anywhere/master/dist/latest";
    public const string InfoUrl = $"{Host}/info.md";
    public const string DownloadUrl = $"{Host}/windows-away-anywhere.zip";
}
